using System;

namespace MyPortfolio.Database.Repositories
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity GetById(int id);
    }
}
