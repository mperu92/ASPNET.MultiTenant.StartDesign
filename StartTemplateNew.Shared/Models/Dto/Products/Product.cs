using StartTemplateNew.Shared.Models.Dto.Base;
using StartTemplateNew.Shared.Models.Dto.Services;
using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.Shared.Models.Dto.Products
{
    public class Product : KeyedDto<Guid>
    {
        public Product() { }

        public Product(Guid id)
            : base(id) { }

        [SetsRequiredMembers]
        public Product(Guid id, string code, string name, string shortDescription, string niceUrl)
            : base(id)
        {
            Code = code;
            Name = name;
            ShortDescription = shortDescription;
            NiceUrl = niceUrl;
        }

        [SetsRequiredMembers]
        public Product(string code, string name, string shortDescription, string niceUrl)
        {
            Code = code;
            Name = name;
            ShortDescription = shortDescription;
            NiceUrl = niceUrl;
        }

        public required string Code { get; set; }
        public required string Name { get; set; }
        public required string ShortDescription { get; set; }
        public required string NiceUrl { get; set; }
        public string? Description { get; set; }

        public Guid ServiceId { get; set; }
        public virtual Service? Service { get; set; }
    }
}
