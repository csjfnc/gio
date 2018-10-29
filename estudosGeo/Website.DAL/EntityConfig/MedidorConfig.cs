using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class MedidorConfig : EntityTypeConfiguration<Medidor>
    {
        public MedidorConfig()
        {
            HasKey(cs => cs.IdMedidor).Property(cs => cs.IdMedidor).HasColumnName("id_medidor").IsRequired();
            Property(cs => cs.IdPontoEntrega).HasColumnName("id_ponto_de_entrega").IsRequired();
            Property(cs => cs.NumeroMedidor).HasColumnName("numero_medidor").HasMaxLength(45);
            Property(cs => cs.ComplementoResidencial).HasColumnName("complemento_residencial").HasMaxLength(45);
            Property(cs => cs.DataExclusao).HasColumnName("data_exclusao");
            
            ToTable("medidores");
        }
    }
}
