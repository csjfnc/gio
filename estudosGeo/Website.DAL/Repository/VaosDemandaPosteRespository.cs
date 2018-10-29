using System;
using System.Data.Entity;
using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class VaosDemandaPosteRespository : GenericRepository<VaosDemandaPoste>, IVaosDemandaPosteRepository
    {
        public VaosDemandaPosteRespository(AppContext appContext) : base(appContext) { }                
    }
}
