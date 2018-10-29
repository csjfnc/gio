using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class FotoArvoreConfig : EntityTypeConfiguration<FotoArvore>
    {
        public FotoArvoreConfig()
        {
            HasKey(f => f.IdFotoArvore).Property(f => f.IdFotoArvore).HasColumnName("id_arvore_fotos").IsRequired();
            Property(f => f.NumeroFoto).HasColumnName("numero_foto").HasMaxLength(10).IsRequired();
            Property(f => f.Path).HasColumnName("patch_foto");
            Property(f => f.CodigoGeoBD).HasColumnName("codigo_bd_geo").IsRequired();
            Property(f => f.IdArvore).HasColumnName("id_arvore").IsRequired();
            Property(f => f.DataExclusao).HasColumnName("data_exclusao");
            Property(f => f.DataFoto).HasColumnName("data_foto");

            ToTable("arvore_fotos");
        }
    }
}