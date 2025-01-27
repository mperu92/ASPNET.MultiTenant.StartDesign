using StartTemplateNew.Shared.Models.Dto.Base.Requests;

namespace StartTemplateNew.Shared.Models.Dto.Requests
{
    public class GetProductsRequest : PaginatedRequest
    {
        public GetProductsRequest()
            : base(1, 100) { }

        public GetProductsRequest(int page, int pageSize)
            : base(page, pageSize) { }
    }
}
