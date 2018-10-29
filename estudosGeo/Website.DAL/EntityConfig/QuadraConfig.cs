using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class QuadraConfig : EntityTypeConfiguration<Quadra>
    {
        public QuadraConfig()
        {
            HasKey(q => q.ID).Property(q => q.ID).HasColumnName("id_quadras").IsRequired();
            Property(q => q.IdOrdemDeServico).HasColumnName("id_ordem_servico");
            Property(q => q.IdCidade).HasColumnName("id_cidade");
            Property(q => q.X1).HasColumnName("x1").IsRequired();
            Property(q => q.Y1).HasColumnName("y1").IsRequired();
            Property(q => q.X2).HasColumnName("x2").IsRequired();
            Property(q => q.Y2).HasColumnName("y2").IsRequired();

            ToTable("quadras");
        }
    }
}
