using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class FotoPontoEntregaConfig : EntityTypeConfiguration<FotoPontoEntrega>
    {
        public FotoPontoEntregaConfig()
        {
            HasKey(f => f.IdFotoPontoEntrega).Property(f => f.IdFotoPontoEntrega).HasColumnName("id_ponto_entregas_fotos").IsRequired();
            Property(f => f.NumeroFoto).HasColumnName("numero_fotos").HasMaxLength(10).IsRequired();
            Property(f => f.Path).HasColumnName("patch_foto");
            Property(f => f.CodigoGeoBD).HasColumnName("codigo_bd_geo").IsRequired();
            Property(f => f.IdPontoEntrega).HasColumnName("id_ponto_de_entrega").IsRequired();
            Property(f => f.DataExclusao).HasColumnName("data_exclusao");
            Property(f => f.DataFoto).HasColumnName("data_foto");

            ToTable("ponto_entregas_fotos");
        }
    }
}
