using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;

namespace Website.MVC.Models.Maps
{
    public class VaoView
    {
        public long IdVaoPrimario { get; set; }
        public double LatitudeA { get; set; }
        public double LongitudeA { get; set; }
        public double LatitudeB { get; set; }
        public double LongitudeB { get; set; }
    }
}