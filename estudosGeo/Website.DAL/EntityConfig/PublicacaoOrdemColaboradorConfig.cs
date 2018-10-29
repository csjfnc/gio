using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class PublicacaoOrdemColaboradorConfig : EntityTypeConfiguration<PublicacaoOrdemColaborador>
    {
        public PublicacaoOrdemColaboradorConfig()
        {
            HasKey(p => p.ID).Property(p => p.ID).HasColumnName("id").IsRequired();
            Property(p => p.IdOrdemDeServico).HasColumnName("id_ordem_servico");
            Property(p => p.Usuario).HasColumnName("usuario");
            Property(p => p.Data_publicado).HasColumnName("data_publicacao");
            Property(p => p.NumeroOs).HasColumnName("numero_os");
            ToTable("publicacao_ordem_servico");
        }
        
    }
}
