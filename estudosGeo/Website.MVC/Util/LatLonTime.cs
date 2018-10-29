using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.BLL.Utils.Geocoding;

namespace Website.MVC.Util
{
    public class LatLonTime
    {
        public int? Panico { get; set; }
        public string Mensagem { get; set; }
        public DateTime Time { get; set; }
        public LatLon LatLon { get; set; }
    }
}