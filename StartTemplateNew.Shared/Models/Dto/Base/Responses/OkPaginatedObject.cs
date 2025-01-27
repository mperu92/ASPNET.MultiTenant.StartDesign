using StartTemplateNew.Shared.Models.Paging;

namespace StartTemplateNew.Shared.Models.Dto.Base.Responses
{
    public class OkPaginatedObject<T>
    {
        private const string _okMessage = "OK";

        public OkPaginatedObject(ICollection<T> data, Pagination pagination)
        {
            Data = new CollectionAndPagination<T>(data, pagination);
        }

        public OkPaginatedObject(ICollection<T> data, Pagination pagination, string? message)
        {
            Data = new CollectionAndPagination<T>(data, pagination);
            if (!string.IsNullOrWhiteSpace(message) && message != _okMessage)
                Message = message;
        }

        public CollectionAndPagination<T> Data { get; }
        public string Message { get; } = _okMessage;
    }
}
