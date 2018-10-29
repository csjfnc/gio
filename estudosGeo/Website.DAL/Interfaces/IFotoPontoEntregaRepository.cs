using Website.BLL.Entities;

namespace Website.DAL.Interfaces
{
    public interface IFotoPontoEntregaRepository : IGenericRepository<FotoPontoEntrega>
    {
        void SetDataExclusao(FotoPontoEntrega Foto);
    }
}
