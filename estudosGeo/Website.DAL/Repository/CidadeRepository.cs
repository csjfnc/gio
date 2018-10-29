using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class CidadeRepository : GenericRepository<Cidade>, ICidadeRepository
    {
        public CidadeRepository(AppContext appContext) : base(appContext) { }
    }
}