using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.MVC.Models.Maps
{
    public class AnotacaoView
    {
        public long IdAnotacao { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Descricao { get; set; }
        public long IdCidade { get; set; }
        public string NumeroOs { get; set; }
    }
}