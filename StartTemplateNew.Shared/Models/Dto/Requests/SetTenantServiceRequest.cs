using StartTemplateNew.Shared.Models.Dto.Base.Requests;

namespace StartTemplateNew.Shared.Models.Dto.Requests
{
    public class SetTenantServiceRequest : TenantedRequest<Guid>
    {
        public Guid ServiceId { get; set; }
        public DateTimeOffset? ActivationDate { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
    }
}
