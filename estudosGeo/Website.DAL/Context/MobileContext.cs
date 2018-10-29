using System.Data.Entity;
using Website.BLL.Entities;
using Website.DAL.EntityConfig;

namespace Website.DAL.Context
{
    public class MobileContext : DbContext
    {
        public MobileContext() : base("DefaultConnection") { }

        public DbSet<Mobile> Mobile { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MobileConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}