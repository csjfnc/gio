using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class FotoArvoreRepository : GenericRepository<FotoArvore>, IFotoArvoreRepository
    {
        public FotoArvoreRepository(AppContext appContext) : base(appContext) { }
    }
}
