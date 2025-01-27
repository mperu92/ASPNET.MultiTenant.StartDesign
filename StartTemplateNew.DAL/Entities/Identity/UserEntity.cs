using Microsoft.AspNetCore.Identity;
using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Entities.Tenant;
using StartTemplateNew.DAL.Helpers;

namespace StartTemplateNew.DAL.Entities.Identity
{
    public class UserEntity : IdentityUser<Guid>, IKeyedEntity<Guid>, IKeyedTenantEntity<TenantEntity, Guid>, IDeleteInfoEntity
    {
        public UserEntity()
        {
            Id = GuidHelper.NewSequentialGuid();
            UserRoles = new HashSet<UserRoleEntity>();
            UserClaims = new HashSet<UserClaimEntity>();
            UserLogins = new HashSet<UserLoginEntity>();
            UserTokens = new HashSet<UserTokenEntity>();
        }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
        public virtual ICollection<UserClaimEntity> UserClaims { get; set; }
        public virtual ICollection<UserLoginEntity> UserLogins { get; set; }
        public virtual ICollection<UserTokenEntity> UserTokens { get; set; }

        public Guid? TenantId { get; set; }
        public virtual TenantEntity? Tenant { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
