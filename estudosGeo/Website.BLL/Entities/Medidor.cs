using System;
using Website.BLL.Enums;

namespace Website.BLL.Entities
{
    public class Medidor
    {
        public long IdMedidor { get; set; }
        public long IdPontoEntrega { get; set; }
        public string NumeroMedidor { get; set; }
        public string ComplementoResidencial { get; set; }
        
        public DateTime? DataExclusao { get; set; }

        public virtual PontoEntrega PontoEntrega { get; set; }
    }
}
