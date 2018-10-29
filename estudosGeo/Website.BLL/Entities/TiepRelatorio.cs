using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.BLL.Entities
{
    public class TiepRelatorio
    {
        public long ID { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public int? Ativo { get; set; }
        public long? DemRel { get; set; }
        public string Layer { get; set; }
        public string OrdemServico { get; set; }

    }
}
