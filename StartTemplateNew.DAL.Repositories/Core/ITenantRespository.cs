using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Repositories.Core.Base;

namespace StartTemplateNew.DAL.Repositories.Core
{
    public interface ITenantRespository : IClaimedRepository<TenantEntity, Guid, UserEntity, Guid>;
}
