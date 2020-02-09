using Microsoft.Extensions.DependencyInjection;
using PostgreSQLIntegration.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyPortfolio.Database.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected PostgreSQLContext _postgreSQLContext;
        private IServiceScopeFactory _scopeFactory;

        public BaseRepository(PostgreSQLContext postgreSQLContext, IServiceScopeFactory scopeFactory)
        {
            _postgreSQLContext = postgreSQLContext;
            _scopeFactory = scopeFactory;
        }

        public void Dispose()
        {
        }

        public IList<TEntity> GetByExpressionMultiThread(Expression<Func<TEntity, bool>> @where)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PostgreSQLContext>();
                return dbContext.Set<TEntity>().AsQueryable().Where(where).ToList();
            }
        }

        public void SaveMultiThreadIncludingSaveContext(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PostgreSQLContext>();
                dbContext.Set<TEntity>().Add(entity);
                dbContext.SaveChanges();
            }
        }

        public IQueryable<TEntity> GetAllSingleThread()
        {
            return _postgreSQLContext.Set<TEntity>().AsQueryable();
        }
    }
}
