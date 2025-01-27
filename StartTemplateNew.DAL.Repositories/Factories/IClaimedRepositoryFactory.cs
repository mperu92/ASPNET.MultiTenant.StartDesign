using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Repositories.Core.Base;

namespace StartTemplateNew.DAL.Repositories.Factories
{
    public interface IClaimedRepositoryFactory
    {
        IClaimedRepository<TEntity, TKey, TClaimUser, TClaimUserKey> GetRepository<TEntity, TKey, TClaimUser, TClaimUserKey>()
            where TEntity : class, IKeyedEntity<TKey>, IAuditedEntity
            where TKey : struct, IEquatable<TKey>
            where TClaimUser : class, IKeyedEntity<TClaimUserKey>
            where TClaimUserKey : struct, IEquatable<TClaimUserKey>;

        TRepository GetClaimedRepoImpl<TRepository>() where TRepository : class, IClaimedRepository;
    }
}
