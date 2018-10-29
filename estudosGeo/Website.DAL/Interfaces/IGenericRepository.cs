using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Website.DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Insert(TEntity obj);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        void Update(TEntity obj);        
        void Delete(TEntity obj);
        void Dispose();
    }
}
