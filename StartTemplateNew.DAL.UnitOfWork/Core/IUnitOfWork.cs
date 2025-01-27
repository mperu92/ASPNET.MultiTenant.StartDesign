using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Entities.Tenant;
using StartTemplateNew.DAL.Repositories.Core.Base;

namespace StartTemplateNew.DAL.UnitOfWork.Core
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class, IKeyedEntity<TKey>
            where TKey : struct, IEquatable<TKey>;

        TRepository GetRepoImpl<TRepository>() where TRepository : class, IRepository;

        IClaimedRepository<TEntity, TKey, TClaimUser, TClaimUserKey> GetClaimedRepository<TEntity, TKey, TClaimUser, TClaimUserKey>()
            where TEntity : class, IKeyedEntity<TKey>, IAuditedEntity
            where TKey : struct, IEquatable<TKey>
            where TClaimUser : class, IKeyedEntity<TClaimUserKey>
            where TClaimUserKey : struct, IEquatable<TClaimUserKey>;
        TRepository GetClaimedRepoImpl<TRepository>()
            where TRepository : class, IClaimedRepository;

        ITenantedRepository<TEntity, TKey, TTenant, TTenantKey, TClaimUser, TClaimUserKey> GetTenantedRepository<TEntity, TKey, TTenant, TTenantKey, TClaimUser, TClaimUserKey>()
            where TEntity : class, IKeyedEntity<TKey>, IKeyedTenantEntity<TTenant, TTenantKey>, IAuditedEntity
            where TKey : struct, IEquatable<TKey>
            where TTenant : class, IKeyedEntity<TTenantKey>
            where TTenantKey : struct, IEquatable<TTenantKey>
            where TClaimUser : class, IKeyedEntity<TClaimUserKey>
            where TClaimUserKey : struct, IEquatable<TClaimUserKey>;
        TRepository GetTenantedRepoImpl<TRepository>()
            where TRepository : class, ITenantedRepository;

        int Commit();
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
