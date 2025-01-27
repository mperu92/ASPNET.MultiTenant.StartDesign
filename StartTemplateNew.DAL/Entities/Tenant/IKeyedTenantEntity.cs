using StartTemplateNew.DAL.Entities.Base;

namespace StartTemplateNew.DAL.Entities.Tenant
{
    public interface IKeyedTenantEntity<TTenant, TTenantKey> : IEntity
        where TTenant : class, IEntity
        where TTenantKey : struct, IEquatable<TTenantKey>
    {
        TTenant? Tenant { get; set; }
        TTenantKey? TenantId { get; set; }
    }
}
