using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.Shared.Models.Dto.Requests
{
    public class CreateRoleRequest
    {
        [SetsRequiredMembers]
        public CreateRoleRequest(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
