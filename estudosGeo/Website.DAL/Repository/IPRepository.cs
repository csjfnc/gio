using System;
using System.Data.Entity;
using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class IPRepository : GenericRepository<IP>, IIPRepository
    {
        public IPRepository(AppContext appContext) : base(appContext) { }

        public void SetDataExclusao(IP Ip)
        {
            Ip.DataExclusao = DateTime.Now;
            AppContext.Entry(Ip).State = EntityState.Modified;
        }
    }
}