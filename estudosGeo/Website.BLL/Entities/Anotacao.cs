using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.BLL.Entities
{
    public class Anotacao
    {
        public long IdAnotacao { get; set; }
        public string Descricao { get; set; }
        public long? IdOrdemDeServico { get; set; }
        public virtual OrdemDeServico OrdemDeServico { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public DateTime? DataExclusao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public long IdCidade { get; set; }
        public virtual Cidade Cidade { get; set; }
        public double Angulo { get; set; }
        public int Ativo { get; set; }
    }
}
