using StartTemplateNew.DAL.Contexts;
using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Entities.Tenant;
using StartTemplateNew.DAL.Repositories.Core.Base;
using StartTemplateNew.DAL.TenantUserProvider.Core;
using StartTemplateNew.DAL.TenantUserProvider.Helpers;

namespace StartTemplateNew.DAL.Repositories.Core.Impl
{
    public class TenantProductRepository : TenantedRepository<TenantProductEntity, int, TenantEntity, Guid, UserEntity, Guid>, ITenantProductRepository
    {
        public TenantProductRepository(ApplicationDbContext dbContext, IPrincipalProvider principalProvider, ITheTypeConverter<Guid> parentConverter, ITheTypeConverter<Guid> converter)
            : base(dbContext, principalProvider, parentConverter, converter) { }
    }
}
