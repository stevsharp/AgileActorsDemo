using AgileActorsDatabaseDemo;
using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;
using AgileActorsDatabaseDemo.IRepository;

namespace AgileActorsDatabaseDemo
{
    public class Repository<T, TId> : IRepository<T, TId> where T : class
    {
        protected readonly BaseDbContext _dbContext;

        public Repository(BaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext
                .Set<T>()
                .ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbContext
                .Set<T>()
                .Where(filter)
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(TId id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
    }
}