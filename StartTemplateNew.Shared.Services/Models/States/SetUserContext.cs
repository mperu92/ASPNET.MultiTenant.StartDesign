using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.Shared.Models.Dto.Identity;
using StartTemplateNew.Shared.Models.Dto.Products;

namespace StartTemplateNew.Shared.Services.Models.States
{
    public class SetUserContext
    {
        public User User { get; set; } = default!;
        public Product Product { get; set; } = default!;
        public UserEntity UserEntity { get; set; } = default!;
        public ProductEntity ProductEntity { get; set; } = default!;
    }
}
