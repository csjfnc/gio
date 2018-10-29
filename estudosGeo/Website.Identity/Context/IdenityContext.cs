using Website.Identity.Model;
using Website.Identity.MySQL;

namespace Website.Identity.Context
{
    /// <summary>
    /// DbContext usado pelo Identity. Recomenda-se que ele fique separado do
    /// contexto da aplicacao caso for usar o EntityFrameowrk. Foi implementado o 
    /// provider do Identity para Postgres e customizados algumas classes
    /// para atender a demanda do projeto.
    /// </summary>
    public class IdentityContext : IdentityContextMysql<ApplicationUser>
    {
        public IdentityContext() : base("DefaultConnection", true) { }

        public static IdentityContext Create()
        {
            return new IdentityContext();
        }
    }
}