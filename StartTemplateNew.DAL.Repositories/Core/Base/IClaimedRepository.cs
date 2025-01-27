using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.TenantUserProvider.Models;

namespace StartTemplateNew.DAL.Repositories.Core.Base
{
    public interface IClaimedRepository;
    public interface IClaimedRepository<TEntity, TKey, TClaimUser, TClaimUserKey> : IRepository<TEntity, TKey>, IClaimedRepository
        where TEntity : class, IKeyedEntity<TKey>, IAuditedEntity
        where TKey : struct, IEquatable<TKey>
        where TClaimUser : class, IKeyedEntity<TClaimUserKey>
        where TClaimUserKey : struct, IEquatable<TClaimUserKey>
    {
        /// <summary>
        /// TClaimUser è mutabile, ma in un contesto dove è registrato tutto scoped, è sicuro restituire il valore direttamente se non ci accedi tramite operazioni parallele
        /// </summary>
        TClaimUser? User { get; }
        IClaimUser ClaimUser { get; }
        bool IsLoggedIn { get; }
        bool IsLoggedInWithTenant { get; }
    }
}
