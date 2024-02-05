using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;
using System.Security.Principal;

namespace AgileActorsDatabaseDemo.Specification
{
    public interface ISpecification<T> where T : class
    {
        Expression<Func<T, bool>> Criteria { get; }
        Expression<Func<T, bool>> And(Expression<Func<T, bool>> query);
        Expression<Func<T, bool>> Or(Expression<Func<T, bool>> query);
    }
}
