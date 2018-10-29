using Website.BLL.Entities;

namespace Website.DAL.Interfaces
{
    public interface IIPRepository : IGenericRepository<IP>
    {
        void SetDataExclusao(IP Ip);
    }
}