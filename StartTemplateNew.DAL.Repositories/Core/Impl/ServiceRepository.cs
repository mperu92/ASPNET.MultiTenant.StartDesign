using StartTemplateNew.DAL.Contexts;
using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Repositories.Core.Base;
using StartTemplateNew.DAL.TenantUserProvider.Core;
using StartTemplateNew.DAL.TenantUserProvider.Helpers;

namespace StartTemplateNew.DAL.Repositories.Core.Impl
{
    public class ServiceRepository : ClaimedRepository<ServiceEntity, Guid, UserEntity, Guid>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext dbContext, IPrincipalProvider principalProvider, ITheTypeConverter<Guid> converter)
            : base(dbContext, principalProvider, converter) { }
    }
}
