using AgileActorsDatabaseDemo;
using AgileActorsDatabaseDemo.Specification;

using System.Linq.Expressions;

namespace AgileActorsDatabaseDemo
{
    public abstract class BaseSpecification<T> : ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>> Criteria { get; protected set; }
       
        public Expression<Func<T, bool>> And(Expression<Func<T, bool>> query)
        {
            return Criteria = Criteria == null ? query : Criteria.And(query);
        }

        public Expression<Func<T, bool>> Or(Expression<Func<T, bool>> query)
        {
            return Criteria = Criteria == null ? query : Criteria.Or(query);
        }
    }
}