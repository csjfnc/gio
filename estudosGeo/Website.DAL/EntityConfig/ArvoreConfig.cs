using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class ArvoreConfig : EntityTypeConfiguration<Arvore>
    {
        public ArvoreConfig()
        {
            HasKey(f => f.IdArvore).Property(f => f.IdArvore).HasColumnName("id_arvore").IsRequired();
            Property(f => f.Logradouro).HasColumnName("logradouro").HasMaxLength(255);
            Property(f => f.Porte).HasColumnName("porte").IsRequired();
            Property(f => f.Localizacao).HasColumnName("localizacao").HasMaxLength(255);
            Property(f => f.X).HasColumnName("x").IsRequired();
            Property(f => f.Y).HasColumnName("y").IsRequired();
            Property(f => f.Ativo).HasColumnName("ativo");
            Property(f => f.NomeBloco).HasColumnName("nomedobloco").HasMaxLength(255);
            Property(f => f.Latitude).HasColumnName("lat");
            Property(f => f.Longitude).HasColumnName("lon");
            Property(f => f.Angulo).HasColumnName("angulo");
            Property(f => f.DataCadastro).HasColumnName("data_cadastro");
            Property(f => f.DataExclusao).HasColumnName("data_exclusao");
            Property(f => f.IdPoste).HasColumnName("id_poste");
            Property(f => f.IdOrdemDeServico).HasColumnName("id_ordem_servico").IsRequired();
            Property(f => f.IdCidade).HasColumnName("id_cidade").IsRequired();

            ToTable("arvores");
        }
    }
}
