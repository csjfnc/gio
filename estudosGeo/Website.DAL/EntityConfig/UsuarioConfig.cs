using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Website.BLL.Entities;

namespace Website.DAL.EntityConfig
{
    public class UsuarioConfig : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfig()
        {
            HasKey(u => u.IdUsuario).Property(os => os.IdUsuario).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .HasColumnName("Id").IsRequired();
            Property(u => u.UserLogin).IsRequired();
            Property(u => u.UserName).IsRequired();
            ToTable("users");

            /// Relacionamento da Classe            
            HasMany(u => u.OrdemDeServico).WithOptional(u => u.Usuario).HasForeignKey(u => u.IdUsuario);
        }
    }
}