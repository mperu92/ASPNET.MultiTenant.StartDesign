using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Entities.Tenant;
using StartTemplateNew.DAL.TenantUserProvider.Models;
using System.Linq.Expressions;

namespace StartTemplateNew.DAL.Repositories.Helpers.Tenant
{
    public static class TenantHandler
    {
        public static IQueryable<TEntity> HandleTenant<TEntity, TKey, TTenant, TTenantKey>(this IQueryable<TEntity> query, TTenantKey? tenantId, IClaimUser claimUser)
            where TEntity : class, IKeyedTenantEntity<TTenant, TTenantKey>, IAuditedEntity
            where TKey : struct, IEquatable<TKey>
            where TTenant : class, IKeyedEntity<TTenantKey>
            where TTenantKey : struct, IEquatable<TTenantKey>
        {
            if (tenantId.HasValue && !claimUser.IsSysAdmin)
                return query.Where(x => x.TenantId.HasValue && x.TenantId.Equals(tenantId));

            return query;
        }

        public static Task HandleTenant<TEntity, TKey, TTenant, TTenantKey>(this TEntity entity, TTenantKey? tenantId, IClaimUser claimUser)
            where TEntity : class, IKeyedEntity<TKey>, IKeyedTenantEntity<TTenant, TTenantKey>, IAuditedEntity
            where TKey : struct, IEquatable<TKey>
            where TTenant : class, IKeyedEntity<TTenantKey>
            where TTenantKey : struct, IEquatable<TTenantKey>
        {
            EnsureTenantSafeness<TEntity, TKey, TTenant, TTenantKey>(entity, tenantId, claimUser);

            if (tenantId.HasValue && !entity.TenantId.HasValue && !claimUser.IsSysAdmin)
                entity.TenantId = tenantId;

            return Task.CompletedTask;
        }

        public static void MergeManyToManyFilterWithTenant<TParentEntity, TTenantKey>(ref Expression<Func<TParentEntity, bool>>? filter, TTenantKey? tenantId, IClaimUser claimUser, Expression<Func<TParentEntity, bool>> manyToManyTenantFilter)
            where TParentEntity : class, IEntity
            where TTenantKey : struct, IEquatable<TTenantKey>
        {
            if (tenantId.HasValue && !claimUser.IsSysAdmin)
            {
                if (filter == null)
                    filter = manyToManyTenantFilter;
                else
                    filter = manyToManyTenantFilter.AndAlso(filter);
            }
        }

        public static Expression<Func<TEntity, bool>>? MergeFilterWithTenant<TEntity, TKey, TTenant, TTenantKey>(Expression<Func<TEntity, bool>>? filter, TTenantKey? tenantId, IClaimUser claimUser)
            where TEntity : class, IKeyedEntity<TKey>, IKeyedTenantEntity<TTenant, TTenantKey>, IAuditedEntity
            where TKey : struct, IEquatable<TKey>
            where TTenant : class, IKeyedEntity<TTenantKey>
            where TTenantKey : struct, IEquatable<TTenantKey>
        {
            if (tenantId.HasValue && !claimUser.IsSysAdmin)
            {
                Expression<Func<TEntity, bool>> tKey = x => x.TenantId.HasValue && x.TenantId.Equals(tenantId);
                if (filter == null)
                    return tKey;
                else
                    return tKey.AndAlso(filter);
            }

            return filter;
        }

        private static void EnsureTenantSafeness<TEntity, TKey, TTenant, TTenantKey>(TEntity entity, TTenantKey? tenantId, IClaimUser claimUser)
                 where TEntity : class, IKeyedEntity<TKey>, IKeyedTenantEntity<TTenant, TTenantKey>, IAuditedEntity
                 where TKey : struct, IEquatable<TKey>
                 where TTenant : class, IKeyedEntity<TTenantKey>
                 where TTenantKey : struct, IEquatable<TTenantKey>
        {
            if (!tenantId.HasValue && !string.IsNullOrEmpty(claimUser.Id) && !claimUser.IsSysAdmin)
                throw new InvalidOperationException("TenantId is not set for the current user.");

            if (tenantId.HasValue && entity.TenantId.HasValue && !entity.TenantId.Equals(tenantId))
                throw new InvalidOperationException("Entity does not belong to the current tenant.");
        }
    }
}
