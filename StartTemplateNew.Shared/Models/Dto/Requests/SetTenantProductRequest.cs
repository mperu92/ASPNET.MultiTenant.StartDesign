using StartTemplateNew.Shared.Models.Dto.Base.Requests;

namespace StartTemplateNew.Shared.Models.Dto.Requests
{
    public class SetTenantProductRequest : TenantedRequest<Guid>
    {
        public Guid ProductId { get; set; }
        public DateTimeOffset? ActivationDate { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
    }
}
