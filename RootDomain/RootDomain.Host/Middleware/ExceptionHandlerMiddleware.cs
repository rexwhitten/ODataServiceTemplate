using Microsoft.Owin;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace RootDomain.Host.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly AppFunc _next;

        public ExceptionHandlerMiddleware(AppFunc next)
        {
            if (next == null)
            {
                throw new ArgumentNullException("next");
            }

            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            try
            {
                await _next(environment);
            }
            catch (Exception ex)
            {
                var owinContext = new OwinContext(environment);

                // Log Adapter
                HandleException(ex, owinContext);
                return;
            }
        }

        private void HandleException(Exception ex, IOwinContext context)
        {
            var request = context.Request;

            //Build a model to represet the error for the client
            //var errorDataModel = NLogLogger.BuildErrorDataModel(ex);
            var errorDataModel = new Hashtable();

            errorDataModel.Add("message", ex.Message);
            errorDataModel.Add("correlationId", Guid.NewGuid());

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ReasonPhrase = "Internal Server Error";
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(errorDataModel));
        }
    }
}