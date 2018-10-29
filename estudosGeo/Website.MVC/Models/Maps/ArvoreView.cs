using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.BLL.Enums;

namespace Website.MVC.Models.Maps
{
    public class ArvoreView
    {
        public ArvoreView() 
        {
            Fotos = new List<FotoArvoreView>();
        }
        public long IdArvore { get; set; }
        public PorteArvore Porte { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<FotoArvoreView> Fotos { get; set; }
    }
}