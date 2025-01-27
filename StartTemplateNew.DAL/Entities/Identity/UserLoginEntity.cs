using Microsoft.AspNetCore.Identity;
using StartTemplateNew.DAL.Entities.Base;

namespace StartTemplateNew.DAL.Entities.Identity
{
    public class UserLoginEntity : IdentityUserLogin<Guid>, IEntity
    {
        public virtual UserEntity User { get; set; } = default!;
    }
}
