using System;
using System.Collections.Generic;
using Website.BLL.Enums;

namespace Website.BLL.Entities
{
    public class VaoPrimario
    {
        public VaoPrimario()
        {

        }

        public long IdVaoPrimario { get; set; }
        public DateTime? DataExclusao { get; set; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }

        public long IdOrdemDeServico { get; set; }
        public virtual OrdemDeServico OrdemDeServico { get; set; }

    }
}
