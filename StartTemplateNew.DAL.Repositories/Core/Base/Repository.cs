using Microsoft.EntityFrameworkCore;
using StartTemplateNew.DAL.Contexts;
using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Repositories.Enums.Queries;
using StartTemplateNew.DAL.Repositories.Exceptions;
using StartTemplateNew.DAL.Repositories.Helpers;
using StartTemplateNew.DAL.Repositories.Helpers.Tenant;
using StartTemplateNew.DAL.Repositories.Models;
using System.Linq.Expressions;

namespace StartTemplateNew.DAL.Repositories.Core.Base
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IKeyedEntity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        protected readonly ApplicationDbContext _dbContext;

        protected DbSet<TEntity> Table { get; }

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Table = _dbContext.Set<TEntity>();
        }

        #region disposing
        private bool _disposed = false;

        ~Repository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext?.Dispose();
                }

                _disposed = true;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true).ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && _dbContext != null)
                {
                    await _dbContext.DisposeAsync().ConfigureAwait(false);
                }

                _disposed = true;
            }
        }

        #endregion

        protected static readonly Expression<Func<TEntity, bool>> _noFilter = _ => true;

        public virtual IQueryable<TEntity> Query => Table;

        public virtual TEntity? GetByFilter(Expression<Func<TEntity, bool>>? filter = null, ICollection<Expression<Func<TEntity, object>>>? includes = null, TypeReturnBehavior returnBehavior = TypeReturnBehavior.FirstOrDefault)
        {
            IQueryable<TEntity> query = Table;
            if (includes?.Count > 0)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }

            return returnBehavior switch
            {
                TypeReturnBehavior.Single => query.Single(filter ?? _noFilter),
                TypeReturnBehavior.SingleOrDefault => query.SingleOrDefault(filter ?? _noFilter),
                TypeReturnBehavior.First => query.First(filter ?? _noFilter),
                TypeReturnBehavior.FirstOrDefault => query.FirstOrDefault(filter ?? _noFilter),
                _ => Table.FirstOrDefault(filter ?? _noFilter),
            };
        }

        public virtual async Task<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>>? filter = null, ICollection<Expression<Func<TEntity, object>>>? includes = null, TypeReturnBehavior returnBehavior = TypeReturnBehavior.FirstOrDefault, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = Table;
            if (includes?.Count > 0)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }

            return returnBehavior switch
            {
                TypeReturnBehavior.Single => await query.SingleAsync(filter ?? _noFilter, cancellationToken).ConfigureAwait(false),
                TypeReturnBehavior.SingleOrDefault => await query.SingleOrDefaultAsync(filter ?? _noFilter, cancellationToken).ConfigureAwait(false),
                TypeReturnBehavior.First => await query.FirstAsync(filter ?? _noFilter, cancellationToken).ConfigureAwait(false),
                TypeReturnBehavior.FirstOrDefault => await query.FirstOrDefaultAsync(filter ?? _noFilter, cancellationToken).ConfigureAwait(false),
                _ => throw new ReturnBehaviorNotFoundException(),
            };
        }

        /// <summary>
        /// Find entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns cref="TEntity?"></returns>
        public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            if (id.Equals(default))
                return null;

            return await Table.FindAsync([id], cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default, CollectionReturnBehavior returnBehavior = CollectionReturnBehavior.ToList)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(pageIndex, 1);
            ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

            IQueryable<TEntity> query = Table;

            if (includes?.Count > 0)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (noTracking)
                query = query.AsNoTracking();

            IQueryable<TEntity> readyToGo = query
                .Where(filter ?? _noFilter)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return returnBehavior switch
            {
                CollectionReturnBehavior.ToList => await readyToGo.ToListAsync(cancellationToken).ConfigureAwait(false),
                CollectionReturnBehavior.ToArray => await readyToGo.ToArrayAsync(cancellationToken).ConfigureAwait(false),
                CollectionReturnBehavior.ToHashSet => await readyToGo.ToHashSetAsync(cancellationToken).ConfigureAwait(false),
                _ => throw new ReturnBehaviorNotFoundException(),
            };
        }

        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<TEntity, object>>>? includes = null)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(pageIndex, 1);
            ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

            IQueryable<TEntity> query = Table;

            if (includes?.Count > 0)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (noTracking)
                query = query.AsNoTracking();

            return query
                .Where(filter ?? _noFilter)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }

        public virtual QueryTotalCountPair<TEntity> GetAllWithCount(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<TEntity, object>>>? includes = null)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(pageIndex, 1);
            ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

            IQueryable<TEntity> query = Table;

            if (includes?.Count > 0)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (noTracking)
                query = query.AsNoTracking();

            int totalCount = query.Count(filter ?? _noFilter);

            IQueryable<TEntity> data = query
                .Where(filter ?? _noFilter)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return new QueryTotalCountPair<TEntity>(data, totalCount);
        }

        public virtual async Task<QueryTotalCountPair<TEntity>> GetAllWithCountAsync(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, CancellationToken cancellationToken = default)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(pageIndex, 1);
            ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

            IQueryable<TEntity> query = Table;
            if (noTracking)
                query = query.AsNoTracking();

            int totalCount = await query.CountAsync(filter ?? _noFilter, cancellationToken).ConfigureAwait(false);

            IQueryable<TEntity> data = query
                .Where(filter ?? _noFilter)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return new QueryTotalCountPair<TEntity>(data, totalCount);
        }
        public virtual async Task<QueryTotalCountPair<TEntity>> GetAllWithCountAsync(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<TEntity, object>>>? includes = null, QueryOrderable<TEntity>? queryOrderable = null, CancellationToken cancellationToken = default)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(pageIndex, 1);
            ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

            IQueryable<TEntity> query = Table;

            if (includes?.Count > 0)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (noTracking)
                query = query.AsNoTracking();

            int totalCount = await query.CountAsync(filter ?? _noFilter, cancellationToken).ConfigureAwait(false);

            IQueryable<TEntity> data = query
                .Where(filter ?? _noFilter)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            if (queryOrderable != null)
                data = data.QueryOrderableOrdering(queryOrderable);

            return new QueryTotalCountPair<TEntity>(data, totalCount);
        }
        public virtual async Task<QueryTotalCountPair<TEntity>> GetAllWithCountAsync(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<string>? includes = null, QueryOrderable<TEntity>? queryOrderable = null, CancellationToken cancellationToken = default)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(pageIndex, 1);
            ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

            IQueryable<TEntity> query = Table;

            if (includes?.Count > 0)
            {
                foreach (string include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (noTracking)
                query = query.AsNoTracking();

            int totalCount = await query.CountAsync(filter ?? _noFilter, cancellationToken).ConfigureAwait(false);

            IQueryable<TEntity> data = query
                .Where(filter ?? _noFilter)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            if (queryOrderable != null)
                data = data.QueryOrderableOrdering(queryOrderable);

            return new QueryTotalCountPair<TEntity>(data, totalCount);
        }

        /// <summary>
        /// Get all entities by filter as async enumerable
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="noTracking"></param>
        /// <returns></returns>
        public virtual IAsyncEnumerable<TEntity> GetAllAsAsyncEnumerable(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<TEntity, object>>>? includes = null)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(pageIndex, 1);
            ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

            IQueryable<TEntity> query = Table;

            if (includes?.Count > 0)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (noTracking)
                query = query.AsNoTracking();

            return query
                .Where(filter ?? _noFilter)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsAsyncEnumerable();
        }

        public virtual void Add(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            entity.HandleAuditing<TEntity, TKey>();

            Table.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (TEntity entity in entities)
            {
                entity.HandleAuditing<TEntity, TKey>();
            }

            Table.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            entity.HandleAuditing<TEntity, TKey>();

            Table.Update(entity);
        }

        public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentNullException.ThrowIfNull(entity);

            entity.HandleAuditing<TEntity, TKey>();

            Table.Update(entity);
            return Task.CompletedTask;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (TEntity entity in entities)
            {
                entity.HandleAuditing<TEntity, TKey>();
            }

            Table.UpdateRange(entities);
        }

        public void Remove(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            Table.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            Table.RemoveRange(entities);
        }

        public virtual bool Any(Expression<Func<TEntity, bool>>? filter = null)
        {
            return Table.Any(filter ?? _noFilter);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            return await Table.AnyAsync(filter ?? _noFilter, cancellationToken).ConfigureAwait(false);
        }

        public virtual int Count(Expression<Func<TEntity, bool>>? filter = null)
        {
            return Table.Count(filter ?? _noFilter);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            return await Table.CountAsync(filter ?? _noFilter, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (entity is ICreateUpdateInfoEntity createUpdateInfoEntity)
            {
                createUpdateInfoEntity.CreatedAt = DateTimeOffset.UtcNow;
                createUpdateInfoEntity.UpdatedAt = DateTimeOffset.UtcNow;
            }

            await Table.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        }

        public bool IsDetached(TEntity entity)
        {
            return _dbContext.Entry(entity).State == EntityState.Detached;
        }

        public bool IsDetached(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            return entities.Any(entity => _dbContext.Entry(entity).State == EntityState.Detached);
        }

        public void Detach(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbContext.Entry(entity).State = EntityState.Detached;
        }

        public void DetachRange(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (TEntity entity in entities)
                _dbContext.Entry(entity).State = EntityState.Detached;
        }

        public void Attach(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            Table.Attach(entity);
        }

        public void AttachRange(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            Table.AttachRange(entities);
        }

        public bool IsReferenceLoaded(TEntity entity, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentException.ThrowIfNullOrEmpty(propertyName);
            return Table.Entry(entity).Reference(propertyName).IsLoaded;
        }

        public Task<bool> IsReferenceLoadedAsync(TEntity entity, string propertyName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentNullException.ThrowIfNull(entity);
            ArgumentException.ThrowIfNullOrEmpty(propertyName);

            bool isLoaded = Table.Entry(entity).Reference(propertyName).IsLoaded;
            return Task.FromResult(isLoaded);
        }

        public bool IsCollectionLoaded(TEntity entity, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentException.ThrowIfNullOrEmpty(propertyName);
            return Table.Entry(entity).Collection(propertyName).IsLoaded;
        }

        public Task<bool> IsCollectionLoadedAsync(TEntity entity, string propertyName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentNullException.ThrowIfNull(entity);
            ArgumentException.ThrowIfNullOrEmpty(propertyName);

            bool isLoaded = Table.Entry(entity).Collection(propertyName).IsLoaded;
            return Task.FromResult(isLoaded);
        }

        public void Collection(TEntity entity, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentException.ThrowIfNullOrEmpty(propertyName);

            Table.Entry(entity).Collection(propertyName).Load();
        }

        public async Task LoadCollectionAsync(TEntity entity, string propertyName, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentException.ThrowIfNullOrEmpty(propertyName);

            await Table.Entry(entity).Collection(propertyName).LoadAsync(cancellationToken).ConfigureAwait(false);
        }

        public void Reference(TEntity entity, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentException.ThrowIfNullOrEmpty(propertyName);

            Table.Entry(entity).Reference(propertyName).Load();
        }

        public async Task ReferenceAsync(TEntity entity, string propertyName, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentException.ThrowIfNullOrEmpty(propertyName);

            await Table.Entry(entity).Reference(propertyName).LoadAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
