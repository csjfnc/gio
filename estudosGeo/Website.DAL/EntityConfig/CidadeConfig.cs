using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class CidadeConfig : EntityTypeConfiguration<Cidade>
    {
        public CidadeConfig()
        {
            HasKey(t => t.IdCidade).Property(t => t.IdCidade).HasColumnName("id_cidade").IsRequired();
            Property(t => t.Nome).HasColumnName("nome_cidade").IsRequired().HasMaxLength(100);
            Property(t => t.Zona).HasColumnName("zona");
            Property(t => t.SetimoDigito).HasColumnName("setimo_digito");
            Property(t => t.NorteOuSul).HasColumnName("norte_sul");
            Property(t => t.Datum).HasColumnName("datum");
            Property(t => t.CidadeDiretorio).HasColumnName("cidade_diretorio");
            Property(t => t.DataExclusao).HasColumnName("data_exclusao");
            ToTable("cidade");

            /// Relacionamento da Classe
            HasMany(c => c.OrdensDeServico).WithRequired(os => os.Cidade).WillCascadeOnDelete(false);
            HasMany(c => c.LimitesCiade).WithRequired(lt => lt.Cidade).WillCascadeOnDelete(false);
        }
    }
}