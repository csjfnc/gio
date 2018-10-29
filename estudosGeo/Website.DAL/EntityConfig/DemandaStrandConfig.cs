using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class DemandaStrandConfig : EntityTypeConfiguration<DemandaStrand>
    {
        public DemandaStrandConfig()
        {
            HasKey(d => d.ID).Property(d => d.ID).HasColumnName("id_strand").IsRequired();
            Property(d => d.IdOrdemDeServico).HasColumnName("id_ordem_servico");
            Property(d => d.IdCidade).HasColumnName("id_cidade");
            Property(d => d.X1).HasColumnName("x1");
            Property(d => d.X2).HasColumnName("x2");
            Property(d => d.Y1).HasColumnName("y1");
            Property(d => d.Y2).HasColumnName("y2");
            Property(d => d.CodigoBdGeo).HasColumnName("codigo_bd_geo");
            Property(d => d.DataExclusao).HasColumnName("data_exclusao");
            Property(d => d.DataInclusao).HasColumnName("data_inclusao");
            Property(d => d.Ativo).HasColumnName("ativo");           
            ToTable("strand");
        }
    }
}
