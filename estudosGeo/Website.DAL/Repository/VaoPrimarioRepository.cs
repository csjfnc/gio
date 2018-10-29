using System;
using System.Data.Entity;
using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class VaoPrimarioRepository : GenericRepository<VaoPrimario>, IVaoPrimarioRepository
    {
        public VaoPrimarioRepository(AppContext appContext) : base(appContext) { }
    }
}