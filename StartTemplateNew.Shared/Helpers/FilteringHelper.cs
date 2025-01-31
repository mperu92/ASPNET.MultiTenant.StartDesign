using Microsoft.EntityFrameworkCore;
using StartTemplateNew.Shared.Exceptions;
using StartTemplateNew.Shared.Models.Dto.Base.Requests;
using System.Linq.Expressions;
using System.Reflection;

namespace StartTemplateNew.Shared.Helpers
{
    public static class FilteringHelper<TEntity>
    {
        public static Expression<Func<TEntity, bool>> CreateExpressionFromSearchField(RequestSearchField searchField)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
            PropertyInfo propertyInfo = typeof(TEntity).GetProperty(searchField.FieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                ?? throw new PropertyNameNotFoundOnEntityException($"Property '{searchField.FieldName}' does not exist on type '{typeof(TEntity).Name}'.");

            MemberExpression property = Expression.Property(parameter, propertyInfo);
            Type propertyType = property.Type;
            object searchValue = Convert.ChangeType(searchField.SearchValue, propertyType);
            ConstantExpression constant = Expression.Constant(searchValue);

            Expression body = searchField.Operator.Value switch
            {
                "eq" => Expression.Equal(property, constant),
                "ne" => Expression.NotEqual(property, constant),
                "gt" => Expression.GreaterThan(property, constant),
                "ge" => Expression.GreaterThanOrEqual(property, constant),
                "lt" => Expression.LessThan(property, constant),
                "le" => Expression.LessThanOrEqual(property, constant),
                "contains" => GetContainsExpression(property, constant, true),
                "startswith" => GetStartsOrEndsWithExpression(property, constant, true),
                "endswith" => GetStartsOrEndsWithExpression(property, constant),
                _ => throw new ArgumentException($"Operator '{searchField.Operator}' is not supported.")
            };

            return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
        }

        private static MethodCallExpression GetStartsOrEndsWithExpression(MemberExpression property, ConstantExpression constant, bool isStartsWith = false)
        {
            if (constant.Value is not string)
                throw new InvalidOperationException($"{(isStartsWith ? "StartsWith" : "EndsWidth")} operator can only be used on string properties.");

            MethodInfo startsOrEndsWithMethod = typeof(string).GetMethod(isStartsWith ? "StartsWith" : "EndsWith", [typeof(string)]) ?? throw new InvalidOperationException();
            return Expression.Call(property, startsOrEndsWithMethod, constant);
        }

        private static MethodCallExpression GetContainsExpression(MemberExpression property, ConstantExpression constant, bool useEfLike = false)
        {
            if (constant.Value is not string)
                throw new InvalidOperationException("Contains operator can only be used on string properties.");

            MethodInfo containsMethod;
            if (!useEfLike)
            {
                containsMethod = typeof(string).GetMethod("Contains", [typeof(string)]) ?? throw new InvalidOperationException();

                return Expression.Call(property, containsMethod, constant);
            }
            else
            {
                MethodInfo likeMethod = typeof(DbFunctionsExtensions).GetMethod("Like", [typeof(DbFunctions), typeof(string), typeof(string)]) ?? throw new InvalidOperationException();
                ConstantExpression dbFunctionsInstance = Expression.Constant(EF.Functions);
                ConstantExpression pattern = Expression.Constant($"%{constant.Value}%");

                return Expression.Call(null, likeMethod, dbFunctionsInstance, property, pattern);
            }
        }
    }
}
