using MyPortfolio.Database.Models;
using PostgreSQLIntegration.Context;
using System;

namespace MyPortfolio.Database.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly PostgreSQLContext _postgreSQLContext;

        public BaseRepository(PostgreSQLContext postgreSQLContext)
        {
            _postgreSQLContext = postgreSQLContext;
        }

        public void Dispose()
        {
        }

        public TEntity GetById(int id)
        {
            return _postgreSQLContext.Set<TEntity>().Find(id);
        }
    }
}
