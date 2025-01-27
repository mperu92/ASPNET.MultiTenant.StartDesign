using StartTemplateNew.Shared.Exceptions;
using StartTemplateNew.Shared.Models.Ordering;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace StartTemplateNew.Shared.Helpers
{
    public static class OrderingHelper<TEntity>
    {
        private static readonly ConcurrentDictionary<OrderableCacheDictionaryKey, Expression<Func<TEntity, object>>> _expressionCache = [];

        /// <summary>
        /// Gets the orering field selector for a given property name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="PropertyNameNotFoundOnEntityException"></exception>
        public static Expression<Func<TEntity, object>> CreateExpressionFromOrderFieldString(string propertyName)
        {
            Type entityType = typeof(TEntity);
            OrderableCacheDictionaryKey key = new(entityType, propertyName);

            return _expressionCache.GetOrAdd(key, _ =>
            {
                PropertyInfo propertyInfo = entityType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ??
                    throw new PropertyNameNotFoundOnEntityException($"Property '{propertyName}' does not exist on type '{typeof(TEntity).Name}'.");

                ParameterExpression parameter = Expression.Parameter(entityType, "entity");
                MemberExpression property = Expression.Property(parameter, propertyInfo);
                UnaryExpression conversion = Expression.Convert(property, typeof(object));

                return Expression.Lambda<Func<TEntity, object>>(conversion, parameter);
            });
        }
    }
}
