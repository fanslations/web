using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Paranovels.DataModels;

namespace Paranovels.DataAccess
{
    public class NovelContext : DbContext
    {
        public NovelContext()
            : base("name=Paranovels")
        {
            Database.SetInitializer<NovelContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(w => w.FullName.Contains("Paranovels.DataModels")))
            {
                var entityTypes = assembly.GetTypes().Where(t =>t.GetCustomAttributes(typeof (TableAttribute), inherit: true).Any());

                foreach (var type in entityTypes)
                {
                    entityMethod.MakeGenericMethod(type)
                      .Invoke(modelBuilder, new object[] { });
                }
            }
        }
    }
}
