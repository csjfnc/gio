using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class LogradouroRepository : GenericRepository<Logradouro>, ILogradouroRepository
    {
        public LogradouroRepository(AppContext appContext) : base(appContext) { }
    }
}
