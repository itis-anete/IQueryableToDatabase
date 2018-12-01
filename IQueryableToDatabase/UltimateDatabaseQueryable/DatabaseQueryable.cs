using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace UltimateDatabaseQueryable
{
    public class DatabaseQueryable<T> : IQueryable<T>
    {
        public DatabaseQueryable()
        {
            Expression = Expression.Constant(this); 
            Provider = new DatabaseQueryProvider();
        }

        public DatabaseQueryable(Expression expression, IQueryProvider provider)
        {
            Expression = expression;
            Provider = provider;
        }

        public Type ElementType => typeof(T);
        public Expression Expression { get; }
        public IQueryProvider Provider { get; }

        public IEnumerator<T> GetEnumerator()
            => ((IEnumerable<T>)Provider.Execute<T>(Expression))
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();
    }
}
