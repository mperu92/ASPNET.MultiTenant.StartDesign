using Microsoft.Extensions.DependencyInjection;
using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Entities.Tenant;
using StartTemplateNew.DAL.Repositories.Core.Base;

namespace StartTemplateNew.DAL.Repositories.Factories.Impl
{
    public class TenantedRepositoryFactory(IServiceProvider serviceProvider) : ITenantedRepositoryFactory
    {
        public ITenantedRepository<TEntity, TKey, TTenant, TTenantKey, TClaimUser, TClaimUserKey> GetRepository<TEntity, TKey, TTenant, TTenantKey, TClaimUser, TClaimUserKey>()
            where TEntity : class, IKeyedEntity<TKey>, IKeyedTenantEntity<TTenant, TTenantKey>, IAuditedEntity
            where TKey : struct, IEquatable<TKey>
            where TTenant : class, IKeyedEntity<TTenantKey>
            where TTenantKey : struct, IEquatable<TTenantKey>
            where TClaimUser : class, IKeyedEntity<TClaimUserKey>
            where TClaimUserKey : struct, IEquatable<TClaimUserKey>
        {
            return (ITenantedRepository<TEntity, TKey, TTenant, TTenantKey, TClaimUser, TClaimUserKey>)serviceProvider.GetRequiredService(typeof(TenantedRepository<TEntity, TKey, TTenant, TTenantKey, TClaimUser, TClaimUserKey>));
        }

        public TRepository GetTenantedRepoImpl<TRepository>()
            where TRepository : class, ITenantedRepository
        {
            TRepository? repository = serviceProvider.GetService<TRepository>();
            return repository
                ?? throw new InvalidOperationException($"Repository of type {typeof(TRepository).Name} is not registered.");
        }
    }
}
