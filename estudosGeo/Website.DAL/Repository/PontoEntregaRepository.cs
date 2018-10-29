using System;
using System.Data.Entity;
using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class PontoEntregaRepository : GenericRepository<PontoEntrega>, IPontoEntregaRepository
    {
        public PontoEntregaRepository(AppContext appContext) : base(appContext) { }
        
        public void SetDataExclusao(PontoEntrega pe)
        {
            pe.DataExclusao = DateTime.Now;
            AppContext.Entry(pe).State = EntityState.Modified;
        }
    }
}
