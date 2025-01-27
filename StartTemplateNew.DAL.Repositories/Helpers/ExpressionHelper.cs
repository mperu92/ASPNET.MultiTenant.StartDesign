using System.Linq.Expressions;

namespace StartTemplateNew.DAL.Repositories.Helpers
{
    public static class ExpressionHelper
    {
        public static Expression<Func<T, bool>>? AndAlso<T>(this Expression<Func<T, bool>>? left, Expression<Func<T, bool>>? right)
        {
            if (left == null)
                return right;
            if (right == null)
                return left;

            ParameterExpression param = Expression.Parameter(typeof(T));
            BinaryExpression body = Expression.AndAlso(
                Expression.Invoke(left, param),
                Expression.Invoke(right, param)
            );

            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        public static Expression<Func<T, bool>>? OrElse<T>(this Expression<Func<T, bool>>? left, Expression<Func<T, bool>>? right)
        {
            if (left == null)
                return right;
            if (right == null)
                return left;

            ParameterExpression param = Expression.Parameter(typeof(T));
            BinaryExpression body = Expression.OrElse(
                Expression.Invoke(left, param),
                Expression.Invoke(right, param)
            );

            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
