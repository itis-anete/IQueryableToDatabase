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
            var result = Translate(expression);
            var command = connection.CreateCommand();
            command.CommandText = result.CommandText;
            var reader = command.ExecuteReader();
            var elementType = TypeSystem.GetElementType(expression.Type);

            if (result.Projector != null)
            {
                var projector = result.Projector.Compile();
                return typeof(ProjectionReader<>)
                    .MakeGenericType(elementType)
                    .GetConstructors()[0]
                    .Invoke(new object[] { reader, projector });
            }
            else
            {
                return typeof(ObjectReader<>)
                    .MakeGenericType(elementType)
                    .GetConstructors()[0]
                    .Invoke(new[] { reader });
            }
        }

        public override string GetQueryString(Expression expression)
            => Translate(expression).CommandText;

        private TranslationResult Translate(Expression expression)
        {
            expression = Evaluator.GetEvaluatedSubTree(expression);
            return new ExpressionTreeToSqlTranslator().Translate(expression);
        }
    }
}