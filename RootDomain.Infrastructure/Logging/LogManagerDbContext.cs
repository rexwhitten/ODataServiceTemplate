namespace RootDomain.Infrastructure.Logging
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class LogManagerDbContext : DbContext, ILogManagerDbContext
    {
        public LogManagerDbContext()
            : base("name=LogManagerDb")
        {
        }

        public virtual DbSet<AppMessageLog> AppMessageLog { get; set; }
        public virtual DbSet<ELMAH_Error> ELMAH_Error { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public Task SaveAllChanges()
        {
            return this.SaveChangesAsync();
        }

        public void Update<T>(T dataModel) where T : class
        {
            this.Entry<T>(dataModel).State = EntityState.Modified;
        }
    }
}
