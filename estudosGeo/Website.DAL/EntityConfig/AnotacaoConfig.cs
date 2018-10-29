using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class AnotacaoConfig : EntityTypeConfiguration<Anotacao>
    {
        public AnotacaoConfig()
        {
            HasKey(a => a.IdAnotacao).Property(a => a.IdAnotacao).HasColumnName("id_anotacoes").IsRequired();
            Property(a => a.IdOrdemDeServico).HasColumnName("id_ordem_servico");
            Property(a => a.IdCidade).HasColumnName("id_cidade");
            Property(a => a.Descricao).HasColumnName("anotacao");
            Property(a => a.X).HasColumnName("x");
            Property(a => a.Y).HasColumnName("y");
            Property(a => a.DataExclusao).HasColumnName("data_exclusao");
            Property(a => a.DataInclusao).HasColumnName("data_inclusao");
            Property(a => a.Ativo).HasColumnName("ativo");
            Property(a => a.Angulo).HasColumnName("angulo");
            ToTable("anotacoes");
        }
    }
}
