using System.Data.Common;
using System.Linq.Expressions;

namespace UltimateDatabaseQueryable
{
    public class DbQueryProvider : QueryProvider
    {
        private readonly DbConnection connection;

        public DbQueryProvider(DbConnection connection)
        {
            this.connection = connection;
        }

        public override object Execute(Expression expression)
        {
            var command = connection.CreateCommand();
            command.CommandText = Translate(expression);
            var reader = command.ExecuteReader();
            var elementType = TypeSystem.GetElementType(expression.Type);

            var result2 = typeof(ObjectReader<>)
                .MakeGenericType(elementType)
                .GetConstructors()[0]
                .Invoke(new[] { reader });

            return result2;
        }

        public override string GetQueryString(Expression expression)
            => Translate(expression);

        private string Translate(Expression expression)
        {
            expression = Evaluator.GetEvaluatedSubTree(expression);
            return new ExpressionTreeToSqlTranslator().Translate(expression);
        }
    }
}