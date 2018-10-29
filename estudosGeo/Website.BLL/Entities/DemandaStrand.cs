using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.BLL.Entities
{
    public class DemandaStrand
    {
        public long ID { get; set; }
        public long? IdOrdemDeServico { get; set; }
        public virtual OrdemDeServico OrdemDeServico { get; set; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }
        public DateTime? DataExclusao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public long CodigoBdGeo { get; set; }
        public long IdCidade { get; set; }
        public virtual Cidade Cidade { get; set; }
        public int Ativo { get; set; }
    }
}
