using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class IPConfig : EntityTypeConfiguration<IP>
    {
        public IPConfig()
        {
            HasKey(ip => ip.IdIp).Property(ip => ip.IdIp).HasColumnName("id_ip").IsRequired();
            Property(ip => ip.TipoBraco).HasColumnName("tipo_braco").HasMaxLength(45);
            Property(ip => ip.TipoLuminaria).HasColumnName("tipo_luminaria").HasMaxLength(45);
            Property(ip => ip.QtdLuminaria).HasColumnName("qtd_luminaria");
            Property(ip => ip.TipoLampada).HasColumnName("tipo_lampada").HasMaxLength(45);
            Property(ip => ip.Potencia).HasColumnName("potencia");
            Property(ip => ip.CodigoGeoBD).HasColumnName("codigo_bd_geo").IsRequired();
            Property(ip => ip.IdPoste).HasColumnName("id_poste").IsRequired();
            Property(ip => ip.IdLogradouro).HasColumnName("id_logradouro");
            Property(ip => ip.Acionamento).HasColumnName("acionamento");
            Property(ip => ip.LampadaAcesa).HasColumnName("lampada_acesa");
            Property(ip => ip.Fase).HasColumnName("fase");
            Property(ip => ip.QtdLampada).HasColumnName("qtd_lampadas");
            Property(ip => ip.DataExclusao).HasColumnName("data_exclusao");
            Property(ip => ip.StatusComparativo).HasColumnName("status_comparativo");
            Property(ip => ip.NumeroPrefeitura).HasColumnName("numero_prefeitura").HasMaxLength(45);

            ToTable("ip");
        }
    }
}