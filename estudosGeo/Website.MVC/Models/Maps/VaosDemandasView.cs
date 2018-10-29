using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.MVC.Models.Maps
{
    public class VaosDemandasView
    {
        public long ID { get; set; }
        public long IdCidade { get; set; }
        public string NumeroOs { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
    }
}