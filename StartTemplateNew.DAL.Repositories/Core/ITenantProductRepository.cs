using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Entities.Tenant;
using StartTemplateNew.DAL.Repositories.Core.Base;

namespace StartTemplateNew.DAL.Repositories.Core
{
    public interface ITenantProductRepository : ITenantedRepository<TenantProductEntity, int, TenantEntity, Guid, UserEntity, Guid>;
}
