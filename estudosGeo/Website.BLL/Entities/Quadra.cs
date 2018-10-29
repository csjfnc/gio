using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.BLL.Utils.Geocoding;

namespace Website.BLL.Entities
{
    public class Quadra
    {
        public long ID { get; set; }

        public long IdOrdemDeServico { get; set; }
        public virtual OrdemDeServico OrdemDeServico { get; set; }

        public long IdCidade { get; set; }
        public virtual Cidade Cidade { get; set; }

        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
    }
}
