using StartTemplateNew.DAL.Contexts;
using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Entities.Tenant;
using StartTemplateNew.DAL.Repositories.Enums.Queries;
using StartTemplateNew.DAL.Repositories.Helpers.Tenant;
using StartTemplateNew.DAL.Repositories.Models;
using StartTemplateNew.DAL.TenantUserProvider.Core;
using StartTemplateNew.DAL.TenantUserProvider.Helpers;
using System.Linq.Expressions;

namespace StartTemplateNew.DAL.Repositories.Core.Base
{
    public class TenantedRepository<TEntity, TKey, TTenant, TTenantKey, TClaimUser, TClaimUserKey> : ClaimedRepository<TEntity, TKey, TClaimUser, TClaimUserKey>, ITenantedRepository<TEntity, TKey, TTenant, TTenantKey, TClaimUser, TClaimUserKey>
        where TEntity : class, IKeyedEntity<TKey>, IKeyedTenantEntity<TTenant, TTenantKey>, IAuditedEntity
        where TKey : struct, IEquatable<TKey>
        where TTenant : class, IKeyedEntity<TTenantKey>
        where TTenantKey : struct, IEquatable<TTenantKey>
        where TClaimUser : class, IKeyedEntity<TClaimUserKey>
        where TClaimUserKey : struct, IEquatable<TClaimUserKey>
    {
        private readonly ITheTypeConverter<TTenantKey> _converter;

        private readonly Lazy<TTenantKey?> _lazyTenantId;

        public TTenantKey? TenantId => _lazyTenantId.Value;

        public TenantedRepository(ApplicationDbContext dbContext, IPrincipalProvider principalProvider, ITheTypeConverter<TClaimUserKey> parentConverter, ITheTypeConverter<TTenantKey> converter)
            : base(dbContext, principalProvider, parentConverter)
        {
            //_tKey = x => x.TenantId.HasValue && x.TenantId.Equals(TenantId)
            _lazyTenantId = new Lazy<TTenantKey?>(() => InitializeTenantKey());
            _converter = converter;
        }

        private TTenantKey? InitializeTenantKey()
        {
            if (!string.IsNullOrEmpty(ClaimUser.TenantId))
                return _converter.ConvertFromInvariantString_n(ClaimUser.TenantId);

            return null;
        }

        public override IQueryable<TEntity> Query => base.Query.HandleTenant<TEntity, TKey, TTenant, TTenantKey>(TenantId, ClaimUser);

        public override TEntity? GetByFilter(Expression<Func<TEntity, bool>>? filter = null, ICollection<Expression<Func<TEntity, object>>>? includes = null, TypeReturnBehavior returnBehavior = TypeReturnBehavior.FirstOrDefault)
        {
            return base.GetByFilter(TenantHandler.MergeFilterWithTenant<TEntity, TKey, TTenant, TTenantKey>(filter, TenantId, ClaimUser), includes, returnBehavior);
        }

        public override async Task<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>>? filter = null, ICollection<Expression<Func<TEntity, object>>>? includes = null, TypeReturnBehavior returnBehavior = TypeReturnBehavior.FirstOrDefault, CancellationToken cancellationToken = default)
        {
            return await base.GetByFilterAsync(TenantHandler.MergeFilterWithTenant<TEntity, TKey, TTenant, TTenantKey>(filter, TenantId, ClaimUser), includes, returnBehavior, cancellationToken).ConfigureAwait(false);
        }

        public override IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<TEntity, object>>>? includes = null)
        {
            return base.GetAll(TenantHandler.MergeFilterWithTenant<TEntity, TKey, TTenant, TTenantKey>(filter, TenantId, ClaimUser), pageIndex, pageSize, noTracking, includes);
        }

        public override async Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default, CollectionReturnBehavior returnBehavior = CollectionReturnBehavior.ToList)
        {
            return await base.GetAllAsync(TenantHandler.MergeFilterWithTenant<TEntity, TKey, TTenant, TTenantKey>(filter, TenantId, ClaimUser), pageIndex, pageSize, noTracking, includes, cancellationToken, returnBehavior).ConfigureAwait(false);
        }

        public override QueryTotalCountPair<TEntity> GetAllWithCount(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<TEntity, object>>>? includes = null)
        {
            return base.GetAllWithCount(TenantHandler.MergeFilterWithTenant<TEntity, TKey, TTenant, TTenantKey>(filter, TenantId, ClaimUser), pageIndex, pageSize, noTracking, includes);
        }

        public override async Task<QueryTotalCountPair<TEntity>> GetAllWithCountAsync(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<TEntity, object>>>? includes = null, QueryOrderable<TEntity>? queryOrderable = null, CancellationToken cancellationToken = default)
        {
            return await base.GetAllWithCountAsync(TenantHandler.MergeFilterWithTenant<TEntity, TKey, TTenant, TTenantKey>(filter, TenantId, ClaimUser), pageIndex, pageSize, noTracking, includes, queryOrderable, cancellationToken).ConfigureAwait(false);
        }

        public override async Task<QueryTotalCountPair<TEntity>> GetAllWithCountAsync(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<string>? includes = null, QueryOrderable<TEntity>? queryOrderable = null, CancellationToken cancellationToken = default)
        {
            return await base.GetAllWithCountAsync(TenantHandler.MergeFilterWithTenant<TEntity, TKey, TTenant, TTenantKey>(filter, TenantId, ClaimUser), pageIndex, pageSize, noTracking, includes, queryOrderable, cancellationToken).ConfigureAwait(false);
        }

        public override async Task<QueryTotalCountPair<TEntity>> GetAllWithCountAsync(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, CancellationToken cancellationToken = default)
        {
            return await base.GetAllWithCountAsync(TenantHandler.MergeFilterWithTenant<TEntity, TKey, TTenant, TTenantKey>(filter, TenantId, ClaimUser), pageIndex, pageSize, noTracking, cancellationToken).ConfigureAwait(false);
        }

        public override IAsyncEnumerable<TEntity> GetAllAsAsyncEnumerable(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<TEntity, object>>>? includes = null)
        {
            return base.GetAllAsAsyncEnumerable(TenantHandler.MergeFilterWithTenant<TEntity, TKey, TTenant, TTenantKey>(filter, TenantId, ClaimUser), pageIndex, pageSize, noTracking, includes);
        }

        public override bool Any(Expression<Func<TEntity, bool>>? filter = null)
        {
            return base.Any(TenantHandler.MergeFilterWithTenant<TEntity, TKey, TTenant, TTenantKey>(filter, TenantId, ClaimUser));
        }

        public override async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            return await base.AnyAsync(TenantHandler.MergeFilterWithTenant<TEntity, TKey, TTenant, TTenantKey>(filter, TenantId, ClaimUser), cancellationToken).ConfigureAwait(false);
        }

        public override int Count(Expression<Func<TEntity, bool>>? filter = null)
        {
            return base.Count(TenantHandler.MergeFilterWithTenant<TEntity, TKey, TTenant, TTenantKey>(filter, TenantId, ClaimUser));
        }

        public override async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            return await base.CountAsync(TenantHandler.MergeFilterWithTenant<TEntity, TKey, TTenant, TTenantKey>(filter, TenantId, ClaimUser), cancellationToken).ConfigureAwait(false);
        }

        public override void Add(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            entity.HandleTenant<TEntity, TKey, TTenant, TTenantKey>(TenantId, ClaimUser);
            Table.Add(entity);
        }

        public async override Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await entity.HandleTenant<TEntity, TKey, TTenant, TTenantKey>(TenantId, ClaimUser).ConfigureAwait(false);
            await Table.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        }

        public override void AddRange(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (TEntity entity in entities)
            {
                entity.HandleTenant<TEntity, TKey, TTenant, TTenantKey>(TenantId, ClaimUser);
            }

            base.AddRange(entities);
        }

        public override void Update(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.HandleTenant<TEntity, TKey, TTenant, TTenantKey>(TenantId, ClaimUser);

            base.Update(entity);
        }

        public async override Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentNullException.ThrowIfNull(entity);

            await entity.HandleTenant<TEntity, TKey, TTenant, TTenantKey>(TenantId, ClaimUser).ConfigureAwait(false);
            await base.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);
        }

        public override void UpdateRange(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (TEntity entity in entities)
            {
                entity.HandleTenant<TEntity, TKey, TTenant, TTenantKey>(TenantId, ClaimUser);
            }

            base.UpdateRange(entities);
        }
    }
}
