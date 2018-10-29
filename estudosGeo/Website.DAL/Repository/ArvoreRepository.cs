using System;
using System.Data.Entity;
using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class ArvoreRepository : GenericRepository<Arvore>, IArvoreRepository
    {
        public ArvoreRepository(AppContext appContext) : base(appContext) { }
    }
}
