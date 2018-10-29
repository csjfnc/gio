using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class LatLonQuadraRepository : GenericRepository<LatLonQuadra>, ILatLonQuadraRepository
    {
        public LatLonQuadraRepository(AppContext appContext) : base(appContext) { }
    }
}
