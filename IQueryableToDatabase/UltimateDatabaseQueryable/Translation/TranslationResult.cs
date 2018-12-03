using System.Linq.Expressions;

namespace UltimateDatabaseQueryable
{
    public class TranslationResult
    {
        public TranslationResult(string commandText, LambdaExpression projector)
        {
            CommandText = commandText;
            Projector = projector;
        }

        public string CommandText { get; }
        public LambdaExpression Projector { get; }
    }
}