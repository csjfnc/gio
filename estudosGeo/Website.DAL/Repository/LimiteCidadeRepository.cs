using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class LimiteCidadeRepository : GenericRepository<LimiteCidade>, ILimiteCidadeRepository
    {
        public LimiteCidadeRepository(AppContext appContext) : base(appContext) { }
    }
}
