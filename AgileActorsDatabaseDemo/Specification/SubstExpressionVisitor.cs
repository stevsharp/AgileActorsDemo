using System.Linq.Expressions;
namespace AgileActorsDatabaseDemo
{
    internal class SubstExpressionVisitor : ExpressionVisitor
    {
        public Dictionary<Expression, Expression> Subst = new();

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (Subst.TryGetValue(node, out var newValue))
            {
                return newValue;
            }
            return node;
        }
    }
}