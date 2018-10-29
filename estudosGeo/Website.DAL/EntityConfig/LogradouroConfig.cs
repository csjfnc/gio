using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class LogradouroConfig : EntityTypeConfiguration<Logradouro>
    {
        public LogradouroConfig() 
        {
            HasKey(log => log.IdLogradouro).Property(log => log.IdLogradouro).HasColumnName("id_logradouro").IsRequired();
            Property(log => log.Tipo).HasColumnName("tipo").HasMaxLength(20).IsRequired();
            Property(log => log.Nome).HasColumnName("nome").HasMaxLength(200).IsRequired();
            Property(log => log.Complemento).HasColumnName("complemento").HasMaxLength(45);
            Property(log => log.Bairro).HasColumnName("bairro").HasMaxLength(45);
            Property(log => log.Cep).HasColumnName("cep");
            ToTable("logradouro");
        }
    }
}
