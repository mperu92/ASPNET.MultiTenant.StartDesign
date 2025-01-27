namespace StartTemplateNew.DAL.Repositories.Models
{
    public readonly struct QueryTotalCountPair<TEntity>(IQueryable<TEntity> query, int count)
    {
        public IQueryable<TEntity> Query { get; } = query;
        public int TotalCount { get; } = count;
    }
}
