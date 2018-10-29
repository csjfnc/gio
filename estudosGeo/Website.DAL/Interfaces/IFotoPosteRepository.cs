using Website.BLL.Entities;

namespace Website.DAL.Interfaces
{
    public interface IFotoPosteRepository : IGenericRepository<FotoPoste>
    {
        void SetDataExclusao(FotoPoste Foto);
    }
}