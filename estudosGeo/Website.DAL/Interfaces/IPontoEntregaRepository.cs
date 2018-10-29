using System;
using Website.BLL.Entities;

namespace Website.DAL.Interfaces
{
    public interface IPontoEntregaRepository : IGenericRepository<PontoEntrega>
    {
        void SetDataExclusao(PontoEntrega ConsumidorBD);
    }
}
