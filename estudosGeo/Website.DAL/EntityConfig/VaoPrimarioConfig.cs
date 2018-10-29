using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class VaoPrimarioConfig : EntityTypeConfiguration<VaoPrimario>
    {
        public VaoPrimarioConfig()
        {
            HasKey(t => t.IdVaoPrimario).Property(t => t.IdVaoPrimario).HasColumnName("id_vao_primario").IsRequired();
            Property(t => t.IdOrdemDeServico).HasColumnName("id_ordem_servico");
            Property(t => t.X1).HasColumnName("x1").IsRequired();
            Property(t => t.X2).HasColumnName("x2").IsRequired();
            Property(t => t.Y1).HasColumnName("y1").IsRequired();
            Property(t => t.Y2).HasColumnName("y2").IsRequired();
            Property(t => t.DataExclusao).HasColumnName("data_exclusao");
            ToTable("vao_primario");
        }
    }
}
