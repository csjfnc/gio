using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class PoligonoOSRepository : GenericRepository<PoligonoOS>, IPoligonoOSRepository
    {
        public PoligonoOSRepository(AppContext appContext) : base(appContext) { }        
    }
}