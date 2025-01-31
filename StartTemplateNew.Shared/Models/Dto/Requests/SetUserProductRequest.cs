using StartTemplateNew.Shared.Models.Dto.Base.Requests;

namespace StartTemplateNew.Shared.Models.Dto.Requests
{
    public class SetUserProductRequest : TenantedRequest<Guid>
    {
        public SetUserProductRequest() { }

        public SetUserProductRequest(Guid productId, DateTimeOffset? activationDate, DateTimeOffset? expirationDate)
        {
            ProductId = productId;
            ActivationDate = activationDate;
            ExpirationDate = expirationDate;
        }

        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }

        public DateTimeOffset? ActivationDate { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
    }
}
