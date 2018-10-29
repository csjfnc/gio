using System;
using System.Data.Entity;
using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class MedidorRepository : GenericRepository<Medidor>, IMedidorRepository
    {
        public MedidorRepository(AppContext appContext) : base(appContext) { }

        public void SetDataExclusao(Medidor medidor)
        {
            medidor.DataExclusao = DateTime.Now;
            AppContext.Entry(medidor).State = EntityState.Modified;
        }
    }
}
