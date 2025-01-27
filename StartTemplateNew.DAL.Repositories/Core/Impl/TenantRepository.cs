using StartTemplateNew.DAL.Contexts;
using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Repositories.Core.Base;
using StartTemplateNew.DAL.TenantUserProvider.Core;
using StartTemplateNew.DAL.TenantUserProvider.Helpers;

namespace StartTemplateNew.DAL.Repositories.Core.Impl
{
    public class TenantRepository : ClaimedRepository<TenantEntity, Guid, UserEntity, Guid>, ITenantRespository
    {
        public TenantRepository(ApplicationDbContext dbContext, IPrincipalProvider principalProvider, ITheTypeConverter<Guid> converter)
            : base(dbContext, principalProvider, converter) { }
    }
}
