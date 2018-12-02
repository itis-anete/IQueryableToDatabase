using System;
using System.Linq;
using System.Linq.Expressions;

namespace UltimateDatabaseQueryable
{
    public abstract class QueryProvider : IQueryProvider
    {
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            => new UltimateQueryable<TElement>(expression, this);

        public IQueryable CreateQuery(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);
            return (IQueryable)Activator.CreateInstance(
                typeof(UltimateQueryable<>).MakeGenericType(elementType), 
                new object[] { expression, this });
        }

        public TResult Execute<TResult>(Expression expression)
            => (TResult)Execute(expression);

        object IQueryProvider.Execute(Expression expression)
            => Execute(expression);

        public abstract string GetQueryString(Expression expression);

        public abstract object Execute(Expression expression);
    }
}