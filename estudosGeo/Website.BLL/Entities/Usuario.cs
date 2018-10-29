using System.Collections.Generic;

namespace Website.BLL.Entities
{
    public class Usuario
    {
        public string IdUsuario { get; set; }
        public string UserName { get; set; }
        public string UserLogin { get; set; }

        public virtual ICollection<OrdemDeServico> OrdemDeServico { get; set; }
    }
}