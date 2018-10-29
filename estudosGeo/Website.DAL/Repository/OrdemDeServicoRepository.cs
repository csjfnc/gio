using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class OrdemDeServicoRepository : GenericRepository<OrdemDeServico>, IOrdemDeServicoRepository
    {
        public OrdemDeServicoRepository(AppContext appContext) : base(appContext) { }       
    }
}
