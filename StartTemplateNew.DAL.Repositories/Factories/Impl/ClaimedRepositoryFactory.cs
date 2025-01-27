using Microsoft.Extensions.DependencyInjection;
using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Repositories.Core.Base;

namespace StartTemplateNew.DAL.Repositories.Factories.Impl
{
    public class ClaimedRepositoryFactory(IServiceProvider serviceProvider) : IClaimedRepositoryFactory
    {
        public IClaimedRepository<TEntity, TKey, TClaimUser, TClaimUserKey> GetRepository<TEntity, TKey, TClaimUser, TClaimUserKey>()
            where TEntity : class, IKeyedEntity<TKey>, IAuditedEntity
            where TKey : struct, IEquatable<TKey>
            where TClaimUser : class, IKeyedEntity<TClaimUserKey>
            where TClaimUserKey : struct, IEquatable<TClaimUserKey>
        {
            return (IClaimedRepository<TEntity, TKey, TClaimUser, TClaimUserKey>)serviceProvider.GetRequiredService(typeof(IClaimedRepository<TEntity, TKey, TClaimUser, TClaimUserKey>));
        }

        public TRepository GetClaimedRepoImpl<TRepository>()
            where TRepository : class, IClaimedRepository
        {
            TRepository? repository = serviceProvider.GetService<TRepository>();
            return repository
                ?? throw new InvalidOperationException($"Repository of type {typeof(TRepository).Name} is not registered.");
        }
    }
}
