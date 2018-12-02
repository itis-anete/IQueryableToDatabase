using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace UltimateDatabaseQueryable
{
    public class SubTreeEvaluator : ExpressionVisitor
    {
        private readonly HashSet<Expression> candidates;

        public SubTreeEvaluator(HashSet<Expression> candidates)
        {
            this.candidates = candidates;
        }

        public Expression EvaluateExpression(Expression expression)
            => Visit(expression);

        public override Expression Visit(Expression expression)
        {
            if (expression == null) return null;
            if (candidates.Contains(expression)) return Evaluate(expression);
            return base.Visit(expression);
        }

        private Expression Evaluate(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Constant)
                return expression;
            var func = Expression.Lambda(expression).Compile();
            return Expression.Constant(func.DynamicInvoke(null), expression.Type);
        }
    }
}
