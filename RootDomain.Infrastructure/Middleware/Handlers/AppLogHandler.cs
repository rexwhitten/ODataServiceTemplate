using Newtonsoft.Json;
using RootDomain.Infrastructure.Logging;
using RootDomain.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Xml;

namespace RootDomain.Infrastructure.Middleware.Handlers
{
    public class AppLogHandler : DelegatingHandler
    {
        private string appName = string.Empty;
        public AppLogHandler(string applicationName)
        {
            appName = applicationName;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpRequestMessage requestMsgClone = await CloneOfHttpRequestMessageWithoutInputStreamAsync(request, false);
            var apiLogEntry = CreateApiLogEntryWithRequestData(request, requestMsgClone, appName);
            if (request.Content != null)
            {
                //Note: Using ContinueWith to avoid deadlock issues experienced while accessing task.results till before completion..
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task =>
                    {
                        //apiLogEntry.RequestContentBody = task.Result;
                    }, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var response = task.Result;

                    // Update the API log entry with response info
                    apiLogEntry.ResponseTimestamp = DateTime.Now;
                    apiLogEntry.ResponseStatusCode = (int)response.StatusCode;

                    if (response.Content != null)
                    {
                        apiLogEntry.ResponseContentType = response.Content.Headers.ContentType.MediaType;
                        apiLogEntry.ResponseContentBody = response.Content.ReadAsStringAsync().Result;
                        apiLogEntry.ResponseHeaders = SerializeHeaders(response.Content.Headers);
                    }

                    // TODO: Save the request log entry into database
                    using (AppMessageLogService logger = new AppMessageLogService())
                    {
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(apiLogEntry.User))
                            {
                                logger.LogData(apiLogEntry);
                            }
                            else if (apiLogEntry.ResponseStatusCode == 401)
                            {
                                //User is unauthorized hence use the machine name as user to log this particular case..
                                if (String.IsNullOrWhiteSpace(apiLogEntry.User))
                                {
                                    apiLogEntry.User = apiLogEntry.Machine;
                                    logger.LogData(apiLogEntry);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string msg = ex.Message;
                        }
                    }

                    return response;
                }, cancellationToken);
        }

        public static async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage req)
        {
            HttpRequestMessage clone = new HttpRequestMessage(req.Method, req.RequestUri);

            // Copy the request's content (via a MemoryStream) into the cloned object
            var ms = new MemoryStream();
            if (req.Content != null)
            {
                await req.Content.CopyToAsync(ms).ConfigureAwait(false);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);

                // Copy the content headers
                if (req.Content.Headers != null)
                    foreach (var h in req.Content.Headers)
                        clone.Content.Headers.Add(h.Key, h.Value);
            }


            clone.Version = req.Version;

            foreach (KeyValuePair<string, object> prop in req.Properties)
                clone.Properties.Add(prop);

            foreach (KeyValuePair<string, IEnumerable<string>> header in req.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            return clone;
        }

        //this is used to get the routeData and routeTemplate info from HttpRequestMessage but without content inputstream
        public async Task<HttpRequestMessage> CloneOfHttpRequestMessageWithoutInputStreamAsync(HttpRequestMessage req, bool readContent)
        {
            HttpRequestMessage clone = new HttpRequestMessage(req.Method, req.RequestUri);

            if (readContent)
            {
                // Copy the request's content (via a MemoryStream) into the cloned object
                var ms = new MemoryStream();

                if (req.Content != null)
                {
                    await req.Content.CopyToAsync(ms).ConfigureAwait(false);
                    ms.Position = 0;
                    clone.Content = new StreamContent(ms);

                    // Copy the content headers
                    if (req.Content.Headers != null)
                        foreach (var h in req.Content.Headers)
                            clone.Content.Headers.Add(h.Key, h.Value);
                }
            }

            clone.Version = req.Version;

            foreach (KeyValuePair<string, object> prop in req.Properties)
                clone.Properties.Add(prop);

            foreach (KeyValuePair<string, IEnumerable<string>> header in req.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            return clone;
        }

        private AppLogEntry CreateApiLogEntryWithRequestData(HttpRequestMessage request, HttpRequestMessage requestClone, string app)
        {
            string routeData = string.Empty;
            IHttpRouteData routeDatainfo = null;
            string routeTmplt = string.Empty;
            var context1 = ((Microsoft.Owin.OwinContext)request.Properties["MS_OwinContext"]); //MS_HttpContext
            HttpContextBase context = context1.Get<HttpContextBase>(typeof(HttpContextBase).FullName);
            try
            {
                if (!request.RequestUri.LocalPath.ToLower().Contains("$metadata"))
                {
                    routeDatainfo = requestClone.GetConfiguration().Routes.GetRouteData(requestClone);
                    routeData = routeDatainfo.Values["controller"].ToString();
                    routeTmplt = routeDatainfo.Route.RouteTemplate;
                }
                else
                {
                    routeData = string.Empty;
                }
            }
            catch (Exception ex)
            {
                routeData = string.Empty;
            }

            AppLogEntry log = new AppLogEntry();
            log.AppMessageEntryId = Guid.NewGuid().ToString();
            log.ApplicationName = app;
            log.User = context.User.Identity.Name;
            log.Machine = Environment.MachineName;
            log.RequestContentType = context.Request.ContentType;
            log.RequestContentBody = context.Request.InputStream.Length > 0 ? RequestBody(context) : "Request Body is empty";
            log.RequestRouteTemplate = !String.IsNullOrWhiteSpace(routeData) ? routeData : "RouteData was empty and it couldn't pull RequestRouteTemplate";
            log.RequestRouteData = !String.IsNullOrWhiteSpace(routeData) ? SerializeRouteData(routeDatainfo) : "RouteData was empty and it couldn't pull RequestRouteData";
            log.RequestIpAddress = context.Request.UserHostAddress;
            log.RequestMethod = request.Method.Method;
            log.RequestHeaders = SerializeHeaders(request.Headers);
            log.RequestTimestamp = DateTime.Now;
            log.RequestUri = request.RequestUri.ToString();

            return log;
        }

        public static string RequestBody(HttpContextBase context)
        {
            StreamReader bodyStream;
            string bodyText = string.Empty;
            try
            {
                bodyStream = new StreamReader(context.Request.InputStream);
                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                bodyText = bodyStream.ReadToEnd();
                context.Request.InputStream.Seek(0, SeekOrigin.Begin); //HttpContext.Current.Request.InputStream.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception ex)
            {
                bodyText = string.Empty;
            }

            return bodyText;
        }

        private string SerializeRouteData(IHttpRouteData routeData)
        {
            if (routeData != null)
                return JsonConvert.SerializeObject(routeData, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            else
                return "RouteData was empty and it couldn't pull RequestRouteData";
        }

        private string SerializeHeaders(HttpHeaders headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value != null)
                {
                    var header = String.Empty;
                    foreach (var value in item.Value)
                    {
                        header += value + " ";
                    }
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }
            return JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
