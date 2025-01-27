using StartTemplateNew.Shared.Models.Dto.Base.Requests;
using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.Shared.Models.Dto.Requests
{
    public class CreateUpdateServiceRequest : BaseCommandRequest<Guid>
    {
        [SetsRequiredMembers]
        public CreateUpdateServiceRequest(string name, string code, string description)
        {
            Name = name;
            Code = code;
            Description = description;
        }

        public required string Name { get; set; }
        public required string Code { get; set; }
        public string? Description { get; set; }
    }
}
