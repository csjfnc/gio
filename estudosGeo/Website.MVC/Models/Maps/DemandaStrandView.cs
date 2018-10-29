using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.MVC.Models.Maps
{
    public class DemandaStrandView
    {
        public long IdDemandaStrand { get; set; }
        //ID de Auxiio do Strandd
        public long ID { get; set; }
        public long IdCidade { get; set; }
        public long IdPoste1 { get; set; }
        public long IdPoste2 { get; set; }
        public string NumeroOs { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }


        public string X1Texto { get; set; }
        public string Y1Texto { get; set; }
        public string X2Texto { get; set; }
        public string Y2Texto { get; set; }
    }
}