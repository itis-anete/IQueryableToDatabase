using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UltimateDatabaseQueryable
{
    public class UltimateQueryable<T> : IQueryable<T>
    {
        public UltimateQueryable(QueryProvider provider)
        {
            Expression = Expression.Constant(this); 
            Provider = provider;
        }

        public UltimateQueryable(Expression expression, QueryProvider provider)
        {
            Expression = expression;
            Provider = provider;
        }

        public Type ElementType => typeof(T);
        public Expression Expression { get; }
        public QueryProvider Provider { get; }
        IQueryProvider IQueryable.Provider => Provider;

        public IEnumerator<T> GetEnumerator()
            => ((IEnumerable<T>)Provider.Execute(Expression))
            .GetEnumerator();

        public override string ToString()
            => Provider.GetQueryString(Expression);

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();
    }
}