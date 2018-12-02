using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace UltimateDatabaseQueryable
{
    public static class Evaluator
    {
        public static Expression GetEvaluatedSubTree(Expression expression,
            Func<Expression, bool> evaluationPredicate)
            => new SubTreeEvaluator(new Nominator(evaluationPredicate).Nominate(expression))
            .EvaluateExpression(expression);

        public static Expression GetEvaluatedSubTree(Expression expression)
            => GetEvaluatedSubTree(expression, CanBeEvaluatedLocally);

        private static bool CanBeEvaluatedLocally(Expression expression)
            => expression.NodeType != ExpressionType.Parameter;
    }
}
