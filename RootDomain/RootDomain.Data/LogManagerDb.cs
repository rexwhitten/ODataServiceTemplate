namespace RootDomain.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LogManagerDb : DbContext
    {
        public LogManagerDb()
            : base("name=LogManagerDb")
        {
        }

        public virtual DbSet<AppMessageLog> AppMessageLogs { get; set; }
        public virtual DbSet<ELMAH_Error> ELMAH_Error { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
