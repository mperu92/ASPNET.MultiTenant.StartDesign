using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Repositories.Core.Base;

namespace StartTemplateNew.DAL.Repositories.Core
{
    public interface IProductRepository : IClaimedRepository<ProductEntity, Guid, UserEntity, Guid>;
}
