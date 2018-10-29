using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Website.Identity.Configuration
{
    /// <summary>
    /// Classe responsavel pela implementacao de envio de email quando 
    /// for criado o usuario ou resetado a senha.
    /// ** Note que nao foi implementado esta funcionadade.
    /// </summary>
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return Task.FromResult(0);
        }
    }
}