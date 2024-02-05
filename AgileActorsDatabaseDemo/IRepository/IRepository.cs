using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace AgileActorsDatabaseDemo.IRepository
{
    public interface IRepository<T, in TId> where T : class
    {
        IQueryable<T> Entities { get; }

        Task<T> GetByIdAsync(TId id);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter);
    }
}
