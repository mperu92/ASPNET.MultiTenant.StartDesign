using StartTemplateNew.Shared.Models.Dto.Base;
using StartTemplateNew.Shared.Models.Dto.Products;
using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.Shared.Models.Dto.Identity
{
    public class User : KeyedDto<Guid>
    {
        [SetsRequiredMembers]
        public User(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }

        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        public virtual ICollection<UserProduct> UserProducts { get; set; } = new HashSet<UserProduct>();
    }
}
