using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class PoligonosOSConfig : EntityTypeConfiguration<PoligonoOS>
    {
        public PoligonosOSConfig()
        {
            HasKey(os => os.IdPoligonoOS).Property(os => os.IdPoligonoOS).HasColumnName("id").IsRequired();
            Property(t => t.X1).HasColumnName("x1").IsRequired();
            Property(t => t.Y1).HasColumnName("y1").IsRequired();
            Property(t => t.X2).HasColumnName("x2").IsRequired();
            Property(t => t.Y2).HasColumnName("y2").IsRequired();
            Property(t => t.Ordem).HasColumnName("ordem").IsRequired();
            Property(t => t.IdOrdemDeServico).HasColumnName("id_ordem_servico");
            Property(t => t.CodigoGeoBD).HasColumnName("codigo_bd_geo");
            ToTable("poligonos_os");
        }
    }
}