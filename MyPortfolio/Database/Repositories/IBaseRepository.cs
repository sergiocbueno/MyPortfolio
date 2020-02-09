using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyPortfolio.Database.Repositories
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
    {
        IList<TEntity> GetByExpressionMultiThread(Expression<Func<TEntity, bool>> @where);
        void SaveMultiThreadIncludingSaveContext(TEntity entity);
        IQueryable<TEntity> GetAllSingleThread();
    }
}
