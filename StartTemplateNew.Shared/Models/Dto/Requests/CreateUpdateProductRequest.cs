using StartTemplateNew.Shared.Models.Dto.Base.Requests;
using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.Shared.Models.Dto.Requests
{
    public class CreateUpdateProductRequest : BaseCommandRequest<Guid>
    {
        [SetsRequiredMembers]
        public CreateUpdateProductRequest(string name, string code, string shortDescription, string description, Guid serviceId)
        {
            Name = name;
            Code = code;
            ShortDescription = shortDescription;
            Description = description;
            ServiceId = serviceId;
        }

        public required string Name { get; set; }
        public required string Code { get; set; }
        public required string ShortDescription { get; set; }
        public string? Description { get; set; }

        public required Guid ServiceId { get; set; }
    }
}
