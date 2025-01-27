using System.ComponentModel;
using System.Linq.Expressions;

namespace StartTemplateNew.DAL.TenantUserProvider.Helpers
{
    public interface ITheTypeConverter<out TKey>
        where TKey : IEquatable<TKey>
    {
        TKey? ConvertFromInvariantString_n(string value);
    }

    public class TheTypeConverter<TKey> : ITheTypeConverter<TKey>
        where TKey : IEquatable<TKey>
    {
        private static readonly Func<string, TKey?> _convert = InitializeConverter();

        public TKey? ConvertFromInvariantString_n(string value) => _convert(value);

        private static Func<string, TKey?> InitializeConverter()
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(TKey));
            if (converter?.CanConvertFrom(typeof(string)) ?? false)
            {
                ParameterExpression param = Expression.Parameter(typeof(string), "value");
                UnaryExpression body = Expression.Convert(Expression.Call(Expression.Constant(converter), "ConvertFromInvariantString", null, param), typeof(TKey));
                return Expression.Lambda<Func<string, TKey?>>(body, param).Compile();
            }
            else
            {
                return _ => default;
            }
        }
    }
}
