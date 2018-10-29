using System;
using System.Data.Entity;
using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class FotoPontoEntregaRepository : GenericRepository<FotoPontoEntrega>, IFotoPontoEntregaRepository
    {
        public FotoPontoEntregaRepository(AppContext appContext) : base(appContext) { }

        public void SetDataExclusao(FotoPontoEntrega Foto)
        {
            Foto.DataExclusao = DateTime.Now;
            AppContext.Entry(Foto).State = EntityState.Modified;
        }
    }
}
