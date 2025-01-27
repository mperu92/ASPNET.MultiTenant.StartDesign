using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Entities.Tenant;

namespace StartTemplateNew.DAL.Repositories.Core.Base
{
    public interface ITenantedRepository;
    public interface ITenantedRepository<TEntity, TKey, TTenant, TTenantKey, TClaimUser, TClaimUserKey> : IClaimedRepository<TEntity, TKey, TClaimUser, TClaimUserKey>, ITenantedRepository
        where TEntity : class, IKeyedEntity<TKey>, IKeyedTenantEntity<TTenant, TTenantKey>, IAuditedEntity
        where TKey : struct, IEquatable<TKey>
        where TTenant : class, IKeyedEntity<TTenantKey>
        where TTenantKey : struct, IEquatable<TTenantKey>
        where TClaimUser : class, IKeyedEntity<TClaimUserKey>
        where TClaimUserKey : struct, IEquatable<TClaimUserKey>
    {
        TTenantKey? TenantId { get; }
    }
}
