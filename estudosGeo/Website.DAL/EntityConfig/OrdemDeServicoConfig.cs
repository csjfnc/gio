using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class OrdemDeServicoConfig : EntityTypeConfiguration<OrdemDeServico>
    {
        public OrdemDeServicoConfig()
        {
            HasKey(os => os.IdOrdemDeServico).Property(os => os.IdOrdemDeServico).HasColumnName("id_ordem_servico").IsRequired();
            Property(os => os.NumeroOS).HasColumnName("numero_os").IsRequired();
            Property(os => os.Situacao).HasColumnName("status").IsRequired();
            Property(os => os.DataInicio).HasColumnName("data_inicio");
            Property(os => os.DataPublicacao).HasColumnName("data_publicacao");
            Property(os => os.DataFinal).HasColumnName("data_fim");
            Property(os => os.IdUsuario).HasColumnName("id_user");
            Property(os => os.IdCidade).HasColumnName("id_cidade");
            Property(os => os.Observacao).HasColumnName("observacao").HasMaxLength(500);
            ToTable("ordem_servico");

            /// Relacionamento da Classe
            HasMany(os => os.PoligonosOS).WithRequired(p => p.OrdemDeServico).WillCascadeOnDelete(false);
            HasMany(os => os.Vaos).WithRequired(v => v.OrdemDeServico).WillCascadeOnDelete(false);
        }
    }
}
