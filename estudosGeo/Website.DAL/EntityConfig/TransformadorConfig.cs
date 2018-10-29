using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class TransformadorConfig : EntityTypeConfiguration<Transformador>
    {
        public TransformadorConfig()
        {
            HasKey(trf => trf.IdTransformador).Property(trf => trf.IdTransformador).HasColumnName("id_transformador").IsRequired();
            Property(trf => trf.CodigoGeoBD).HasColumnName("codigo_bd_geo").IsRequired();
            Property(trf => trf.IdPoste).HasColumnName("id_poste").IsRequired();
            Property(trf => trf.Status).HasColumnName("status").HasMaxLength(45);
            Property(trf => trf.Proprietario).HasColumnName("proprietario").HasMaxLength(45);
            Property(trf => trf.Fase).HasColumnName("fase").HasMaxLength(20);
            Property(trf => trf.NumeroCampo).HasColumnName("numero_campo").HasMaxLength(45);
            Property(trf => trf.PotenciaTotal).HasColumnName("potencia_total").HasMaxLength(45);
            Property(trf => trf.TipoLigacao).HasColumnName("tipo_ligacao").HasMaxLength(45);
            Property(trf => trf.TensaoNominal).HasColumnName("tensao_nominal");
            Property(trf => trf.TipoInstalacao).HasColumnName("tipo_instalacao").HasMaxLength(45);
            Property(trf => trf.CortaCircuito).HasColumnName("corta_circuito").HasMaxLength(45);
            Property(trf => trf.Descricao).HasColumnName("descricao").HasMaxLength(45);
            Property(trf => trf.DataExclusao).HasColumnName("data_exclusao");
            Property(trf => trf.NumeroEquipamento).HasColumnName("numero_equipamento");

            ToTable("transformador");
        }
    }
}