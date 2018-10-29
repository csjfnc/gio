using Website.BLL.Entities;

namespace Website.DAL.Interfaces
{
    public interface IPosteRepository : IGenericRepository<Poste>
    {
        /// <summary>
        /// Seta data de Exclusao ao Poste
        /// </summary>
        void SetDataExclusao(Poste Poste);
    }
}