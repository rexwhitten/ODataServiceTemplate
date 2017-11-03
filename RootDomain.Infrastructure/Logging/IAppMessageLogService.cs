using RootDomain.Infrastructure.Utility;
using RootDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootDomain.Infrastructure.Logging
{
    public interface IAppMessageLogService
    {
        void LogData(AppLogEntry log);
    }
}
