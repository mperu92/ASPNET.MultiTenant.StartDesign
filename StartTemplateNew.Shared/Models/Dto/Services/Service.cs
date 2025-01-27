using StartTemplateNew.Shared.Models.Dto.Base;
using StartTemplateNew.Shared.Models.Dto.Products;
using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.Shared.Models.Dto.Services
{
    public class Service : KeyedDto<Guid>
    {
        [SetsRequiredMembers]
        public Service(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public required string Name { get; set; }
        public required string Code { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
