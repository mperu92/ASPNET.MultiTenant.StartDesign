using Microsoft.EntityFrameworkCore;
using StartTemplateNew.DAL.Contexts;
using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Entities.Tenant;
using StartTemplateNew.DAL.Repositories.Core.Base;
using StartTemplateNew.DAL.Repositories.Helpers.Tenant;
using StartTemplateNew.DAL.Repositories.Models;
using StartTemplateNew.DAL.TenantUserProvider.Core;
using StartTemplateNew.DAL.TenantUserProvider.Helpers;
using System.Linq.Expressions;

namespace StartTemplateNew.DAL.Repositories.Core.Impl
{
    public class TenantServiceRepository : TenantedRepository<TenantServiceEntity, int, TenantEntity, Guid, UserEntity, Guid>, ITenantServiceRepository
    {
        public TenantServiceRepository(ApplicationDbContext dbContext, IPrincipalProvider principalProvider, ITheTypeConverter<Guid> parentConverter, ITheTypeConverter<Guid> converter)
            : base(dbContext, principalProvider, parentConverter, converter) { }

        private static readonly Expression<Func<ServiceEntity, bool>> _servicesNoFilter = _ => true;

        public async Task<QueryTotalCountPair<ServiceEntity>> GetAllWithCountAsync(Expression<Func<ServiceEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<ServiceEntity, object>>>? includes = null, CancellationToken cancellationToken = default)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(pageIndex, 1);
            ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

            IQueryable<ServiceEntity> query = _dbContext.Services;
            if (noTracking)
                query = query.AsNoTracking();

            if (includes?.Count > 0)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            TenantHandler.MergeManyToManyFilterWithTenant(ref filter, TenantId, ClaimUser, x => x.TenantServices.Any(x => x.TenantId == TenantId));

            int totalCount = await query.CountAsync(filter ?? _servicesNoFilter, cancellationToken).ConfigureAwait(false);

            IQueryable<ServiceEntity> data = query
                .Where(filter ?? _servicesNoFilter)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return new QueryTotalCountPair<ServiceEntity>(data, totalCount);
        }
    }
}
