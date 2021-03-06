﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootDomain.Data.Logging
{
    public interface ILogManagerDb
    {
        DbSet<ELMAH_Error> ELMAH_Error { get; set; }
        DbSet<AppMessageLog> AppMessageLog { get; set; }

        Task SaveAllChanges();

        void Update<T>(T dataModel) where T : class;
    }
}
