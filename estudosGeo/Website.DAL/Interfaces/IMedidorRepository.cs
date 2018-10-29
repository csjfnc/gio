using System;
using Website.BLL.Entities;


namespace Website.DAL.Interfaces
{
    public interface IMedidorRepository : IGenericRepository<Medidor>
    {
        void SetDataExclusao(Medidor Medidor);
    }
}
