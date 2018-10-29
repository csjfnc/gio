using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class LimiteCidadeConfig : EntityTypeConfiguration<LimiteCidade>
    {
        public LimiteCidadeConfig()
        {
            HasKey(lt => lt.IdLimeteCidade).Property(os => os.IdLimeteCidade).HasColumnName("id_limite_cidade").IsRequired();
            Property(lt => lt.X1).HasColumnName("x1").IsRequired();
            Property(lt => lt.Y1).HasColumnName("y1").IsRequired();
            Property(lt => lt.X2).HasColumnName("x2").IsRequired();
            Property(lt => lt.Y2).HasColumnName("y2").IsRequired();
            Property(lt => lt.IdCidade).HasColumnName("id_cidade");
            Property(lt => lt.CodigoGeoBD).HasColumnName("codigo_bd_geo");

            ToTable("limite_cidade");
        }
    }
}