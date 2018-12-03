using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace UltimateDatabaseQueryable
{
    public class ExpressionTreeToSqlTranslator : ExpressionVisitor
    {
        private StringBuilder sqlRequestBuilder;
        private ParameterExpression row;
        private ColumnProjection projection;

        public TranslationResult Translate(Expression expression)
        {
            sqlRequestBuilder = new StringBuilder();
            row = Expression.Parameter(typeof(ProjectionRow), "row");
            Visit(expression);
            return new TranslationResult(
                sqlRequestBuilder.ToString(),
                projection != null ? Expression.Lambda(projection.Selector, row) : null);
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            switch (expression.Method.Name)
            {
                case "Where":
                    return HandleWhereMethod(expression);
                case "Select":
                    return HandleSelectMethod(expression);
                default:
                    throw new NotSupportedException(
                        $"The method '{expression.Method.Name}' is not supported.");
            }
        }

        protected override Expression VisitUnary(UnaryExpression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Not:
                    return HandleNotOperator(expression);
                default:
                    throw new NotSupportedException(
                        $"The unary operator '{expression.NodeType}' is not supported.");
            }
        }

        protected override Expression VisitBinary(BinaryExpression expression)
        {
            sqlRequestBuilder.Append("(");
            Visit(expression.Left);
            HandleBinaryOperators(expression);
            Visit(expression.Right);
            sqlRequestBuilder.Append(")");
            return expression;
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            if (expression.Value is IQueryable queryable)
            {
                sqlRequestBuilder.Append("select * from ");
                sqlRequestBuilder.Append(queryable.ElementType.Name);
            }
            else if (expression.Value == null)
            {
                sqlRequestBuilder.Append("null");
            }
            else
            {
                HandleConstant(expression);
            }
            return expression;
        }

        protected override Expression VisitMember(MemberExpression expression)
        {
            if (expression?.Expression.NodeType == ExpressionType.Parameter)
            {
                sqlRequestBuilder.Append(expression.Member.Name);
                return expression;
            }
            throw new NotSupportedException(
                $"The member '{expression.Member.Name}' is not supported.");
        }

        private MethodCallExpression HandleWhereMethod(MethodCallExpression expression)
        {
            sqlRequestBuilder.Append("select * from (");
            Visit(expression.Arguments[0]);
            sqlRequestBuilder.Append(") as T where ");
            var lambda = GetInnerExpressionFromQuote(expression.Arguments[1]);
            Visit(lambda.Body);
            return expression;
        }

        private MethodCallExpression HandleSelectMethod(MethodCallExpression expression)
        {
            var lambda = GetInnerExpressionFromQuote(expression.Arguments[1]);
            var projection = new ColumnProjector().ProjectColumns(lambda.Body, row);
            sqlRequestBuilder.Append("select ");
            sqlRequestBuilder.Append(projection.Columns.Length > 0 ? projection.Columns : "*");
            sqlRequestBuilder.Append(" from (");
            Visit(expression.Arguments[0]);
            sqlRequestBuilder.Append(") as T ");
            this.projection = projection;
            return expression;
        }

        private UnaryExpression HandleNotOperator(UnaryExpression expression)
        {
            sqlRequestBuilder.Append(" not ");
            Visit(expression.Operand);
            return expression;
        }

        private void HandleBinaryOperators(BinaryExpression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.And:
                    sqlRequestBuilder.Append(" and ");
                    break;
                case ExpressionType.Or:
                    sqlRequestBuilder.Append(" or ");
                    break;
                case ExpressionType.AndAlso:
                    sqlRequestBuilder.Append(" and ");
                    break;
                case ExpressionType.Equal:
                    sqlRequestBuilder.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    sqlRequestBuilder.Append(" <> ");
                    break;
                case ExpressionType.GreaterThan:
                    sqlRequestBuilder.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    sqlRequestBuilder.Append(" >= ");
                    break;
                case ExpressionType.LessThan:
                    sqlRequestBuilder.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    sqlRequestBuilder.Append(" <= ");
                    break;
                default:
                    throw new NotSupportedException(
                        $"The binary operator '{expression.NodeType}' is not supported.");
            }
        }

        private void HandleConstant(ConstantExpression expression)
        {
            switch (expression.Value)
            {
                case bool flag:
                    sqlRequestBuilder.Append(flag ? 1 : 0);
                    break;
                case string str:
                    sqlRequestBuilder.Append("'");
                    sqlRequestBuilder.Append(str);
                    sqlRequestBuilder.Append("'");
                    break;
                default:
                    sqlRequestBuilder.Append(expression.Value);
                    break;
            }
        }

        private static LambdaExpression GetInnerExpressionFromQuote(Expression expression)
        {
            while (expression.NodeType == ExpressionType.Quote)
            {
                expression = (expression as UnaryExpression).Operand;
            }
            return expression as LambdaExpression;
        }
    }
}