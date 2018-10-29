using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class GenericRepository<TEntity> : IDisposable, IGenericRepository<TEntity> where TEntity : class
    {
        protected AppContext AppContext;

        /// <summary>
        /// Contrutor usado quando for usado apenas o repositório.
        /// </summary>
        public GenericRepository()
        {
            AppContext = new AppContext();
        }
        
        /// <summary>
        /// Construtor usado quando o repositório for usado pela UnitOfWork.
        /// </summary>
        /// <param name="AppContext"></param>
        public GenericRepository(AppContext AppContext)
        {
            this.AppContext = AppContext;
        }
        
        public void Insert(TEntity obj)
        {
            AppContext.Set<TEntity>().Add(obj);
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
       {
            IQueryable<TEntity> query = AppContext.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public void Update(TEntity obj)
        {
            AppContext.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(TEntity obj)
        {
            AppContext.Set<TEntity>().Remove(obj);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    AppContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}