using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Paranovels.DataModels;

namespace Paranovels.DataAccess
{
    public partial class SqlViewContext : DbContext
    {
        public SqlViewContext()
            : base("name=Paranovels")
        {
            Database.SetInitializer<SqlViewContext>(null);
        }

        //public DbSet<CmsGranteeReport> CmsGranteeReports { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<CmsGranteeReport>().ToTable("vwCmsGranteeReport").HasKey(m => m.RecordId);
        }
    }
}
