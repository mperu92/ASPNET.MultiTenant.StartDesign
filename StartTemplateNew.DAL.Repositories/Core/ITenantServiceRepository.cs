using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Entities.Tenant;
using StartTemplateNew.DAL.Repositories.Core.Base;
using StartTemplateNew.DAL.Repositories.Models;
using System.Linq.Expressions;

namespace StartTemplateNew.DAL.Repositories.Core
{
    public interface ITenantServiceRepository : ITenantedRepository<TenantServiceEntity, int, TenantEntity, Guid, UserEntity, Guid>
    {
        Task<QueryTotalCountPair<ServiceEntity>> GetAllWithCountAsync(Expression<Func<ServiceEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<ServiceEntity, object>>>? includes = null, CancellationToken cancellationToken = default);
    }
}
