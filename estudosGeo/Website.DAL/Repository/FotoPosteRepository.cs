using System;
using System.Data.Entity;
using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class FotoPosteRepository : GenericRepository<FotoPoste>, IFotoPosteRepository
    {
        public FotoPosteRepository(AppContext appContext) : base(appContext) { }

        public void SetDataExclusao(FotoPoste Foto)
        {
            Foto.DataExclusao = DateTime.Now;
            AppContext.Entry(Foto).State = EntityState.Modified;
        }
    }
}