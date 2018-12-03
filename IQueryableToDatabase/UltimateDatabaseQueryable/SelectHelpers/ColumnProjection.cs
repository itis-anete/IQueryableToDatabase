using System.Linq.Expressions;

namespace UltimateDatabaseQueryable
{
    public class ColumnProjection
    {
        public ColumnProjection(string columns, Expression selector)
        {
            Columns = columns;
            Selector = selector;
        }

        public string Columns { get; }
        public Expression Selector { get; }
    }
}