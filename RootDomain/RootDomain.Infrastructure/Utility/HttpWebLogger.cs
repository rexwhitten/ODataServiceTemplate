using RootDomain.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootDomain.Infrastructure.Utility
{
    public class HttpWebLogger : IDisposable
    {
        public HttpWebLogger()
        {
            //default constructor...
        }

        #region Log Request and Response
        public void LogWebRequest(AppLogEntry appLogEntry)
        {
            try
            {
                using (LogManagerDbContext db = new LogManagerDbContext())
                {
                    if (appLogEntry != null)
                    {
                        AppMessageLog msg = new AppMessageLog();
                        msg.AppMessageEntryId = appLogEntry.AppMessageEntryId;
                        msg.ApplicationName = appLogEntry.ApplicationName;
                        msg.User = appLogEntry.User;
                        msg.Machine = appLogEntry.Machine;
                        msg.RequestIpAddress = appLogEntry.RequestIpAddress;
                        msg.RequestContentType = appLogEntry.RequestContentType;
                        msg.RequestContentBody = appLogEntry.RequestContentBody;
                        msg.RequestUri = appLogEntry.RequestUri;
                        msg.RequestMethod = appLogEntry.RequestMethod;
                        msg.RequestRouteTemplate = appLogEntry.RequestRouteTemplate;
                        msg.RequestRouteData = appLogEntry.RequestRouteData;
                        msg.RequestHeaders = appLogEntry.RequestHeaders;
                        msg.RequestTimestamp = appLogEntry.RequestTimestamp;
                        msg.ResponseContentType = appLogEntry.ResponseContentType;
                        msg.ResponseContentBody = appLogEntry.ResponseContentBody;
                        msg.ResponseStatusCode = appLogEntry.ResponseStatusCode;
                        msg.ResponseHeaders = appLogEntry.ResponseHeaders;
                        msg.ResponseTimestamp = appLogEntry.ResponseTimestamp;

                        db.AppMessageLog.Add(msg);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                disposedValue = true;
            }
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~HttpWebLogger() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
