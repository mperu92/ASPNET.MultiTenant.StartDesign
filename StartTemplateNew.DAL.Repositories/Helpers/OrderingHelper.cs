using StartTemplateNew.DAL.Repositories.Models;

namespace StartTemplateNew.DAL.Repositories.Helpers
{
    internal static class OrderingHelper
    {
        public static IQueryable<TEntity> QueryOrderableOrdering<TEntity>(this IQueryable<TEntity> source, QueryOrderable<TEntity> queryOrderable)
            where TEntity : class
        {
            return queryOrderable.IsAscending
                ? source.OrderBy(queryOrderable.FieldSelector)
                : source.OrderByDescending(queryOrderable.FieldSelector);
        }
    }
}
