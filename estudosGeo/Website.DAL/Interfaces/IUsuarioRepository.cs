using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Website.BLL.Entities;

namespace Website.DAL.Interfaces
{
    public interface IUsuarioRepository
    {
        IEnumerable<Usuario> Get(Expression<Func<Usuario, bool>> filter = null, Func<IQueryable<Usuario>, IOrderedQueryable<Usuario>> orderBy = null, string includeProperties = "");
    }
}