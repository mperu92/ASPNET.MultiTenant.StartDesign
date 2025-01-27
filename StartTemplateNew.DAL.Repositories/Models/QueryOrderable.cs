using System.Linq.Expressions;

namespace StartTemplateNew.DAL.Repositories.Models
{
    public class QueryOrderable<TEntity>
        where TEntity : class
    {
        public QueryOrderable(Expression<Func<TEntity, object>> fieldSelector, bool isAscending)
        {
            FieldSelector = fieldSelector;
            IsAscending = isAscending;
        }

        public Expression<Func<TEntity, object>> FieldSelector { get; init; }
        public bool IsAscending { get; init; }
    }
}
