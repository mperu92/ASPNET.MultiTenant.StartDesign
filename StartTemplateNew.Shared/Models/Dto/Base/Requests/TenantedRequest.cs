namespace StartTemplateNew.Shared.Models.Dto.Base.Requests
{
    public class TenantedRequest<TKey> : ITenantedRequest<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        public TKey TenantId { get; set; }
    }
}
