using System;
using System.Data.Entity;
using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class PosteRepository : GenericRepository<Poste>, IPosteRepository
    {
        public PosteRepository(AppContext appContext) : base(appContext) { }

        public void SetDataExclusao(Poste poste)
        {
            poste.DataExclusao = DateTime.Now;
            AppContext.Entry(poste).State = EntityState.Modified;
        }
    }
}