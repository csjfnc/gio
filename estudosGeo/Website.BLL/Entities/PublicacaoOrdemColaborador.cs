using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.BLL.Entities
{
    public class PublicacaoOrdemColaborador
    {
        public long ID { get; set; }        
        public long IdOrdemDeServico { get; set; }
        public virtual OrdemDeServico OrdemDeServico { get; set; }
        public string Usuario { get; set; }
        //public virtual Usuario Usuario { get; set; }
        public string Data_publicado { get; set; }
        public string NumeroOs { get; set; }
    }
}
