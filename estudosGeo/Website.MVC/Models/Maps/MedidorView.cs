using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.MVC.Models.Maps
{
    public class MedidorView
    {
        public long IdMedidor { get; set; }
        public long IdPontoEntrega { get; set; }
        public string NumeroMedidor { get; set; }
        public string ComplementoResidencial { get; set; }
    }
}