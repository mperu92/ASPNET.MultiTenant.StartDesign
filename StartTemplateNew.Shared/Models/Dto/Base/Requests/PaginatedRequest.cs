using StartTemplateNew.Shared.Models.Paging;

namespace StartTemplateNew.Shared.Models.Dto.Base.Requests
{
    public class PaginatedRequest : BaseQueryRequest
    {
        public PaginatedRequest()
            => Pagination = Pagination.Default;

        public PaginatedRequest(int page, int pageSize)
            => Pagination = Pagination.Create(page, pageSize);

        public Pagination Pagination { get; set; }
    }
}
