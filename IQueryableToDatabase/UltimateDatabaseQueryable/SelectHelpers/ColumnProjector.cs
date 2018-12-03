using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace UltimateDatabaseQueryable
{
    public class ColumnProjector : ExpressionVisitor
    {
        private StringBuilder stringBuilder;
        private int columnNumber;
        private ParameterExpression row;
        private static MethodInfo methodInfoGetValue;

        public ColumnProjector()
        {
            methodInfoGetValue = typeof(ProjectionRow).GetMethod("GetValue");
        }

        public ColumnProjection ProjectColumns(Expression expression, ParameterExpression row)
        {
            stringBuilder = new StringBuilder();
            this.row = row;
            var selector = Visit(expression);
            return new ColumnProjection(stringBuilder.ToString(), selector);
        }

        protected override Expression VisitMember(MemberExpression expression)
        {
            if (expression.Expression != null &&
                expression.Expression.NodeType == ExpressionType.Parameter)
            {
                if (stringBuilder.Length > 0) stringBuilder.Append(", ");
                stringBuilder.Append(expression.Member.Name);
                return Expression.Convert(
                    Expression.Call(
                        row,
                        methodInfoGetValue,
                        Expression.Constant(columnNumber++)),
                    expression.Type);
            }
            else return base.VisitMember(expression);
        }
    }
}