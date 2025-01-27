using Microsoft.AspNetCore.Identity;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.DAL.Entities.Identity
{
    [SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "Want to specify HashSet type")]
    public class RoleEntity : IdentityRole<Guid>, IKeyedEntity<Guid>
    {
        [SetsRequiredMembers]
        public RoleEntity(string description)
        {
            Id = GuidHelper.NewSequentialGuid();
            UserRoles = new HashSet<UserRoleEntity>();
            RoleClaims = new HashSet<RoleClaimEntity>();
            Description = description;
        }

        [StringLength(50)]
        public required string Description { get; set; }

        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
        public virtual ICollection<RoleClaimEntity> RoleClaims { get; set; }
    }
}
