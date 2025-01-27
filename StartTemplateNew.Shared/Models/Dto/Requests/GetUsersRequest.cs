using StartTemplateNew.Shared.Models.Dto.Base.Requests;

namespace StartTemplateNew.Shared.Models.Dto.Requests
{
    public class GetUsersRequest : PaginatedRequest
    {
        public GetUsersRequest()
            : base(1, 100) { }

        public GetUsersRequest(int page, int pageSize)
            : base(page, pageSize) { }
    }
}
