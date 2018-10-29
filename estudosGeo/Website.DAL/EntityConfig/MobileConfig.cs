using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class MobileConfig: EntityTypeConfiguration<Mobile>
    {
        public MobileConfig()
        {
            HasKey(t => t.Id).Property(t => t.Id).IsRequired();
            Property(t => t.Imei).IsRequired();
            Property(t => t.Name).IsRequired();
            Property(t => t.PhoneNumber).IsRequired().HasMaxLength(11).IsFixedLength();
            Property(t => t.Version).HasMaxLength(45);
            
            ToTable("mobile");
        }
    }
}
