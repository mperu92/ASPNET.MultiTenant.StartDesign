namespace StartTemplateNew.Shared.Models.Dto.Base.Requests
{
    public interface ITenantedRequest<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        TKey TenantId { get; set; }
    }
}
