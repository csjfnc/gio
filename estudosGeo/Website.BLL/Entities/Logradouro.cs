using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.BLL.Entities
{
    public class Logradouro
    {
        public long IdLogradouro { get; set; }
        public string Tipo { get; set; }
        public string Nome { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
    }
}
