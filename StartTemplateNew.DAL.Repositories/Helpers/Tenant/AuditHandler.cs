using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;

namespace StartTemplateNew.DAL.Repositories.Helpers.Tenant
{
    public static class AuditHandler
    {
        public static Task HandleAuditing<TEntity, TKey, TClaimUser, TClaimUserKey>(this TEntity entity, TClaimUser? user,  bool isLoggedIn = false)
            where TEntity : class, IKeyedEntity<TKey>
            where TKey : struct, IEquatable<TKey>
            where TClaimUser : class, IKeyedEntity<TClaimUserKey>
            where TClaimUserKey : struct, IEquatable<TClaimUserKey>
        {
            if (isLoggedIn)
            {
                if (entity is IKeyedCreateUpdateInfoEntity<TClaimUser> keyedCreateUpdateInfoEntity)
                {
                    if (keyedCreateUpdateInfoEntity.CreatedAt == default)
                    {
                        keyedCreateUpdateInfoEntity.CreatedBy = user!;
                    }

                    keyedCreateUpdateInfoEntity.UpdatedBy = user;
                }

                if (entity is IKeyedDeleteInfoEntity<TClaimUser> keyedDeleteInfoEntity && keyedDeleteInfoEntity.IsDeleted)
                {
                    keyedDeleteInfoEntity.DeletedBy = user;
                }

                if (entity is IKeyedActivationExpiringInfoEntity<TClaimUser> tenantEntity)
                {
                    if (tenantEntity.ActivationDate != default)
                        tenantEntity.ActivationSetBy = user;
                    if (tenantEntity.ExpirationDate != default)
                        tenantEntity.ExpirationSetBy = user;
                }
                else if (entity is IKeyedExpiringInfoEntity<TClaimUser> expiringInfoEntity && expiringInfoEntity.ExpirationDate != default)
                {
                    expiringInfoEntity.ExpirationSetBy = user;
                }
            }

            return HandleAuditing<TEntity, TKey>(entity);
        }

        public static Task HandleAuditing<TEntity, TKey>(this TEntity entity)
            where TEntity : class, IKeyedEntity<TKey>
            where TKey : struct, IEquatable<TKey>
        {
            if (entity is ICreateUpdateInfoEntity createUpdateInfoEntity)
            {
                if (createUpdateInfoEntity.CreatedAt == default)
                    createUpdateInfoEntity.CreatedAt = DateTimeOffset.UtcNow;

                createUpdateInfoEntity.UpdatedAt = DateTimeOffset.UtcNow;
            }

            if (entity is IDeleteInfoEntity deleteInfoEntity && deleteInfoEntity.IsDeleted)
                deleteInfoEntity.DeletedAt = DateTimeOffset.UtcNow;

            return Task.CompletedTask;
        }
    }
}
