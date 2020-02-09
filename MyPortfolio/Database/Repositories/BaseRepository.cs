using Microsoft.Extensions.DependencyInjection;
using PostgreSQLIntegration.Context;
using System.Linq;

namespace MyPortfolio.Database.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public BaseRepository(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void Dispose()
        {
        }

        public TEntity GetById(int id)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PostgreSQLContext>();
                dbContext.Set<TEntity>().AsQueryable();
                return dbContext.Set<TEntity>().Find(id);
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PostgreSQLContext>();
                return dbContext.Set<TEntity>().AsQueryable();
            }
        }
    }
}
