using Microsoft.AspNetCore.Identity;
using StartTemplateNew.DAL.Entities.Base;

namespace StartTemplateNew.DAL.Entities.Identity
{
    public class UserClaimEntity : IdentityUserClaim<Guid>, IKeyedEntity<int>
    {
        public virtual UserEntity User { get; set; } = default!;
    }
}
