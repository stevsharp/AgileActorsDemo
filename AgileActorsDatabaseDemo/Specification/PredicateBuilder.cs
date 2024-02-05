
using System.Linq.Expressions;

namespace AgileActorsDatabaseDemo
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            ParameterExpression p = left.Parameters.First();
            
            SubstExpressionVisitor visitor = new()
            {
                Subst = { [right.Parameters.First()] = p }
            };

            Expression body = Expression.AndAlso(left.Body, visitor.Visit(right.Body));

            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {

            ParameterExpression p = left.Parameters.First();
            
            SubstExpressionVisitor visitor = new()
            {
                Subst = { [right.Parameters.First()] = p }
            };

            Expression body = Expression.OrElse(left.Body, visitor.Visit(right.Body));

            return Expression.Lambda<Func<T, bool>>(body, p);
        }
    }
}