using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace UltimateDatabaseQueryable
{
    public class ExpressionTreeToSqlTranslator : ExpressionVisitor
    {
        private readonly StringBuilder sqlRequestBuilder;

        public ExpressionTreeToSqlTranslator()
        {
            sqlRequestBuilder = new StringBuilder();
        }

        public string Translate(Expression expression)
        {
            Visit(expression);
            return sqlRequestBuilder.ToString();
        }
    }
}
