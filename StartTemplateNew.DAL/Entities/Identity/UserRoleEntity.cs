using Microsoft.AspNetCore.Identity;
using StartTemplateNew.DAL.Entities.Base;

namespace StartTemplateNew.DAL.Entities.Identity
{
    public class UserRoleEntity : IdentityUserRole<Guid>, IEntity
    {
        public virtual UserEntity User { get; set; } = default!;
        public virtual RoleEntity Role { get; set; } = default!;
    }
}
