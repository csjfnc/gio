using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.BLL.Entities
{
    public class LatLonQuadra
    {
        public long ID { get; set; }
        public long QuadraID { get; set; }
        //public virtual Quadra Quadra { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
