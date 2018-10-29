using System;
using System.Data.Entity;
using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class TransformadorRepository : GenericRepository<Transformador>, ITransformadorRepository
    {
        public TransformadorRepository(AppContext appContext) : base(appContext) { }

        public void SetDataExclusao(Transformador transformador)
        {
            transformador.DataExclusao = DateTime.Now;
            AppContext.Entry(transformador).State = EntityState.Modified;
        }
    }
}