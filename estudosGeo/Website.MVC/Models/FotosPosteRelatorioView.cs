using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.MVC.Models
{
    public class FotosPosteRelatorioView
    {
        public long CodBDGeoPoste { get; set; }
        public string NumeroFoto { get; set; }
        public string DiretorioFoto { get; set; }
        public string NumeroOS { get; set; }
        public string ExisteHD { get; set; }
    }
}