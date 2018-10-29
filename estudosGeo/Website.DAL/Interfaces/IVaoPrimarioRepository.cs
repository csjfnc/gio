using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Website.BLL.Entities;

namespace Website.DAL.Interfaces
{
    public interface IVaoPrimarioRepository
    {
        IEnumerable<VaoPrimario> Get(Expression<Func<VaoPrimario, bool>> filter = null, Func<IQueryable<VaoPrimario>, IOrderedQueryable<VaoPrimario>> orderBy = null, string includeProperties = "");
    }
}
