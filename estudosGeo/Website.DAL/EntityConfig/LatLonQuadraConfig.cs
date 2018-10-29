using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class LatLonQuadraConfig : EntityTypeConfiguration<LatLonQuadra>
    {
        public LatLonQuadraConfig()
        {
            HasKey(lat => lat.ID).Property(lat => lat.ID).HasColumnName("id_lat_lon_quadras").IsRequired();
            Property(lat => lat.QuadraID).HasColumnName("id_quadras");
            Property(lat => lat.X).HasColumnName("X");
            Property(lat => lat.Y).HasColumnName("Y");
            ToTable("lat_lon_quadras");
        }
    }
}
