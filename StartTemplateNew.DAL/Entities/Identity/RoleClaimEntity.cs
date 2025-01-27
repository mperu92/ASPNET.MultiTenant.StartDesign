using Microsoft.AspNetCore.Identity;
using StartTemplateNew.DAL.Entities.Base;

namespace StartTemplateNew.DAL.Entities.Identity
{
    public class RoleClaimEntity : IdentityRoleClaim<Guid>, IKeyedEntity<int>
    {
        public virtual RoleEntity Role { get; set; } = default!;
    }
}
