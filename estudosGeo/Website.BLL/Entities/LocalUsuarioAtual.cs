using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.BLL.Utils.Geocoding;

namespace Website.BLL.Entities
{
    public class LocalUsuarioAtual
    {
        public long ID { get; set; }
        public string NomeUser { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public DateTime TimeBanco { get; set; }
        public int? Panico { get; set; }
        public string Mensagem { get; set; }
    }
}