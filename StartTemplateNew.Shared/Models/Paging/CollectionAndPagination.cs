namespace StartTemplateNew.Shared.Models.Paging
{
    public class CollectionAndPagination<T>(ICollection<T> value, Pagination pagination)
    {
        public ICollection<T> Values { get; } = value;
        public Pagination Pagination { get; } = pagination;
    }
}
