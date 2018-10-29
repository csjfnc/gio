using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class FotoPosteConfig : EntityTypeConfiguration<FotoPoste>
    {
        public FotoPosteConfig()
        {
            HasKey(f => f.IdFotoPoste).Property(f => f.IdFotoPoste).HasColumnName("id_postes_fotos").IsRequired();
            Property(f => f.NumeroFoto).HasColumnName("numero_foto").HasMaxLength(10).IsRequired();
            Property(f => f.Path).HasColumnName("patch_foto");
            Property(f => f.CodigoGeoBD).HasColumnName("codigo_bd_geo").IsRequired();
            Property(f => f.IdPoste).HasColumnName("id_poste").IsRequired();
            Property(f => f.DataExclusao).HasColumnName("data_exclusao");
            Property(f => f.DataFoto).HasColumnName("data_foto");
            
            ToTable("postes_fotos");
        }
    }
}