using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Repositories.Enums.Queries;
using StartTemplateNew.DAL.Repositories.Models;
using System.Linq.Expressions;

namespace StartTemplateNew.DAL.Repositories.Core.Base
{
    public interface IRepository : IDisposable, IAsyncDisposable;

    public interface IRepository<TEntity, in TKey> : IRepository
        where TEntity : class, IKeyedEntity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        IQueryable<TEntity> Query { get; }
        TEntity? GetByFilter(Expression<Func<TEntity, bool>>? filter = null, ICollection<Expression<Func<TEntity, object>>>? includes = null, TypeReturnBehavior returnBehavior = TypeReturnBehavior.FirstOrDefault);
        Task<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>>? filter = null, ICollection<Expression<Func<TEntity, object>>>? includes = null, TypeReturnBehavior returnBehavior = TypeReturnBehavior.FirstOrDefault, CancellationToken cancellationToken = default);
        Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<TEntity, object>>>? includes = null);
        Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<TEntity, object>>>? includes = null, CancellationToken cancellationToken = default, CollectionReturnBehavior returnBehavior = CollectionReturnBehavior.ToList);
        QueryTotalCountPair<TEntity> GetAllWithCount(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<TEntity, object>>>? includes = null);
        Task<QueryTotalCountPair<TEntity>> GetAllWithCountAsync(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, CancellationToken cancellationToken = default);
        Task<QueryTotalCountPair<TEntity>> GetAllWithCountAsync(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<TEntity, object>>>? includes = null, QueryOrderable<TEntity>? queryOrderable = null, CancellationToken cancellationToken = default);
        Task<QueryTotalCountPair<TEntity>> GetAllWithCountAsync(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<string>? includes = null, QueryOrderable<TEntity>? queryOrderable = null, CancellationToken cancellationToken = default);
        IAsyncEnumerable<TEntity> GetAllAsAsyncEnumerable(Expression<Func<TEntity, bool>>? filter = null, int pageIndex = 1, int pageSize = 100, bool noTracking = false, ICollection<Expression<Func<TEntity, object>>>? includes = null);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        bool Any(Expression<Func<TEntity, bool>>? filter = null);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default);
        int Count(Expression<Func<TEntity, bool>>? filter = null);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        bool IsDetached(TEntity entity);
        bool IsDetached(IEnumerable<TEntity> entities);
        void Detach(TEntity entity);
        void DetachRange(IEnumerable<TEntity> entities);
        void Attach(TEntity entity);
        void AttachRange(IEnumerable<TEntity> entities);
        void Collection(TEntity entity, string propertyName);
        Task LoadCollectionAsync(TEntity entity, string propertyName, CancellationToken cancellationToken = default);
        void Reference(TEntity entity, string propertyName);
        Task ReferenceAsync(TEntity entity, string propertyName, CancellationToken cancellationToken = default);
        bool IsReferenceLoaded(TEntity entity, string propertyName);
        Task<bool> IsReferenceLoadedAsync(TEntity entity, string propertyName, CancellationToken cancellationToken = default);
        bool IsCollectionLoaded(TEntity entity, string propertyName);
        Task<bool> IsCollectionLoadedAsync(TEntity entity, string propertyName, CancellationToken cancellationToken = default);
    }
}
