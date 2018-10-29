using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.BLL.Entities; 

namespace Website.DAL.EntityConfig
{
    public class LocalUsuarioAtualConfig : EntityTypeConfiguration<LocalUsuarioAtual>
    {
        public LocalUsuarioAtualConfig()
        {
            HasKey(l => l.ID).Property(l => l.ID).HasColumnName("id").IsRequired();
            Property(l => l.NomeUser).HasColumnName("nome");
            Property(l => l.X).HasColumnName("x");
            Property(l => l.Y).HasColumnName("y");
            Property(l => l.TimeBanco).HasColumnName("data");
            Property(l => l.Panico).HasColumnName("panico");
            Property(l => l.Mensagem).HasColumnName("mensagem"); 

            ToTable("localuser");
        }
    }
}
