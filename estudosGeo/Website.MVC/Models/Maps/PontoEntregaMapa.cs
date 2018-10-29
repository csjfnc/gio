using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.MVC.Models.Maps
{
    public class PontoEntregaMapa
    {
        public double ponto_entrega_latitude { get; set; }
        public double ponto_entrega_longitude { get; set; }
        public double poste_latitude { get; set; }
        public double poste_longitude { get; set; }
        public string IdPontoEntrega { get; set; }
        public string Image { get; set; }
    }
}