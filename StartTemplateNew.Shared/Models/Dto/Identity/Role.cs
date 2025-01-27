using StartTemplateNew.Shared.Models.Dto.Base;
using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.Shared.Models.Dto.Identity
{
    public class Role : KeyedDto<Guid>
    {
        [SetsRequiredMembers]
        public Role(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public required string Name { get; set; }
        public required string Description { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }
}
