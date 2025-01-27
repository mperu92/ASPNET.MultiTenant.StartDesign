using Newtonsoft.Json;

namespace StartTemplateNew.Shared.Models.Paging
{
    public class Pagination
    {
        private const int _defaultPage = 1;
        private const int _defaultPageSize = 100;

        public Pagination() { }

        public Pagination(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; set; } = _defaultPage;
        public int PageSize { get; set; } = _defaultPageSize;

        [JsonIgnore]
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public static Pagination Default => new();

        public Pagination SetTotalCount(int totalItems)
        {
            TotalItems = totalItems;
            return this;
        }

        public static Pagination Create(int page, int pageSize)
            => new(page, pageSize);
    }
}
