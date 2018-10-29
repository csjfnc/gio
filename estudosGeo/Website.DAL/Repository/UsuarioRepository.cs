using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Website.BLL.Entities;
using Website.DAL.Context;
using Website.DAL.Interfaces;

namespace Website.DAL.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        protected AppContext AppContext;

        public UsuarioRepository(AppContext appContext)
        {
            AppContext = appContext;
        }

        public IEnumerable<Usuario> Get(Expression<Func<Usuario, bool>> filter = null, Func<IQueryable<Usuario>, IOrderedQueryable<Usuario>> orderBy = null, string includeProperties = "")
        {
            IQueryable<Usuario> query = AppContext.Set<Usuario>();

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
    }
}