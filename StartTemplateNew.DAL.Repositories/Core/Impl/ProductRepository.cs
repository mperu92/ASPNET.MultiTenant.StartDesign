using StartTemplateNew.DAL.Contexts;
using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Repositories.Core.Base;
using StartTemplateNew.DAL.TenantUserProvider.Core;
using StartTemplateNew.DAL.TenantUserProvider.Helpers;

namespace StartTemplateNew.DAL.Repositories.Core.Impl
{
    public class ProductRepository : ClaimedRepository<ProductEntity, Guid, UserEntity, Guid>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext, IPrincipalProvider principalProvider, ITheTypeConverter<Guid> principalKeysTypeConverter)
            : base(dbContext, principalProvider, principalKeysTypeConverter) { }
    }
}
