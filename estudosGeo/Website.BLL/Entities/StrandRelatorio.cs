using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.BLL.Entities
{
    public class StrandRelatorio
    {
        public long ID { get; set; }
        public long OrdemServicoID { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double Lat1 { get; set; }
        public double Lat2 { get; set; }
        public double Lon1 { get; set; }
        public double Lon2 { get; set; }
        public int Ativo { get; set; }
        public long CodigoBDGeo { get; set; }
        public string Cidade { get; set; }
        public string OrdemServico { get; set; }
        public string Colaborador { get; set; }
    }
}
