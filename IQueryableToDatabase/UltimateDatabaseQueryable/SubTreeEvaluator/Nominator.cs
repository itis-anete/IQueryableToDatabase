using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace UltimateDatabaseQueryable
{
    public class Nominator : ExpressionVisitor
    {
        private readonly Func<Expression, bool> evaluationPredicate;
        private HashSet<Expression> candidates;
        private bool cannotBeEvaluated;

        public Nominator(Func<Expression, bool> evaluationPredicate)
        {
            this.evaluationPredicate = evaluationPredicate;
        }

        public HashSet<Expression> Nominate(Expression expression)
        {
            candidates = new HashSet<Expression>();
            Visit(expression);
            return candidates;
        }

        public override Expression Visit(Expression expression)
        {
            if (expression != null)
            {
                var saveCannotBeEvaluated = cannotBeEvaluated;
                cannotBeEvaluated = false;
                base.Visit(expression);
                if (!cannotBeEvaluated)
                {
                    if (evaluationPredicate(expression)) candidates.Add(expression);
                    else cannotBeEvaluated = true;
                }
                cannotBeEvaluated |= saveCannotBeEvaluated;
            }
            return expression;
        }
    }
}
