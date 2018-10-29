using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class VaosDemandaPosteConfig : EntityTypeConfiguration<VaosDemandaPoste>
    {
        public VaosDemandaPosteConfig()
        {           
            HasKey(v => v.IdVaosDemandaPoste).Property(v => v.IdVaosDemandaPoste).HasColumnName("id_vaos_demanda_poste").IsRequired();
            Property(v => v.IdPontoEntrega).HasColumnName("id_ponto_entrega");
            Property(v => v.IdOrdemDeServico).HasColumnName("id_ordem_servico");
            Property(v => v.IdPoste).HasColumnName("id_poste");
            Property(v => v.X1).HasColumnName("x1");
            Property(v => v.X2).HasColumnName("x2");
            Property(v => v.Y1).HasColumnName("y1");
            Property(v => v.Y2).HasColumnName("y2");
            Property(v => v.DataExclusao).HasColumnName("data_exclusao");
            Property(v => v.Ativo).HasColumnName("ativo");
            Property(v => v.IdCidade).HasColumnName("id_cidade");
            Property(v => v.DataInclusao).HasColumnName("data_inclusao");
            ToTable("vaos_demanda_poste");

        }
    }
}
