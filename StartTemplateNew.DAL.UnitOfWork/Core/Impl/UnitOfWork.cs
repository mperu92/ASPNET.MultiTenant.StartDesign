using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StartTemplateNew.DAL.Contexts;
using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Entities.Tenant;
using StartTemplateNew.DAL.Repositories.Core.Base;
using StartTemplateNew.DAL.Repositories.Factories;
using StartTemplateNew.DAL.UnitOfWork.Exceptions;
using StartTemplateNew.Shared.ExceptionHelpers.Core;

namespace StartTemplateNew.DAL.UnitOfWork.Core.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IClaimedRepositoryFactory _claimedRepositoryFactory;
        private readonly ITenantedRepositoryFactory _tenantRepositoryFactory;

        public UnitOfWork(ApplicationDbContext dbContext, IRepositoryFactory repositoryFactory, IClaimedRepositoryFactory claimedRepositoryFactory, ITenantedRepositoryFactory tenantedRepositoryFactory)
        {
            DbContext = dbContext;
            _repositoryFactory = repositoryFactory;
            _claimedRepositoryFactory = claimedRepositoryFactory;
            _tenantRepositoryFactory = tenantedRepositoryFactory;
        }

        protected ApplicationDbContext DbContext { get; }

        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class, IKeyedEntity<TKey>
            where TKey : struct, IEquatable<TKey>
        {
            return _repositoryFactory.GetRepository<TEntity, TKey>();
        }

        public TRepository GetRepoImpl<TRepository>() where TRepository : class, IRepository
        {
            return _repositoryFactory.GetSpecificRepositoryInstance<TRepository>();
        }

        public IClaimedRepository<TEntity, TKey, TClaimUser, TClaimUserKey> GetClaimedRepository<TEntity, TKey, TClaimUser, TClaimUserKey>()
            where TEntity : class, IKeyedEntity<TKey>, IAuditedEntity
            where TKey : struct, IEquatable<TKey>
            where TClaimUser : class, IKeyedEntity<TClaimUserKey>
            where TClaimUserKey : struct, IEquatable<TClaimUserKey>
        {
            return _claimedRepositoryFactory.GetRepository<TEntity, TKey, TClaimUser, TClaimUserKey>();
        }

        public TRepository GetClaimedRepoImpl<TRepository>()
            where TRepository : class, IClaimedRepository
        {
            return _claimedRepositoryFactory.GetClaimedRepoImpl<TRepository>();
        }

        public ITenantedRepository<TEntity, TKey, TTenant, TTenantKey, TClaimUser, TClaimUserKey> GetTenantedRepository<TEntity, TKey, TTenant, TTenantKey, TClaimUser, TClaimUserKey>()
            where TEntity : class, IKeyedEntity<TKey>, IKeyedTenantEntity<TTenant, TTenantKey>, IAuditedEntity
            where TKey : struct, IEquatable<TKey>
            where TTenant : class, IKeyedEntity<TTenantKey>
            where TTenantKey : struct, IEquatable<TTenantKey>
            where TClaimUser : class, IKeyedEntity<TClaimUserKey>
            where TClaimUserKey : struct, IEquatable<TClaimUserKey>
        {
            return _tenantRepositoryFactory.GetRepository<TEntity, TKey, TTenant, TTenantKey, TClaimUser, TClaimUserKey>();
        }

        public TRepository GetTenantedRepoImpl<TRepository>()
            where TRepository : class, ITenantedRepository
        {
            return _tenantRepositoryFactory.GetTenantedRepoImpl<TRepository>();
        }

        public virtual int Commit()
        {
            try
            {
                return DbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (ex.GetBaseException() is SqlException sqlException)
                {
                    sqlException.HandleSqlException<UnitOfWorkTransactionException>();
                    throw;
                }
                else
                {
                    throw new UnitOfWorkTransactionException($"An error occurred while saving changes to the database.\n{ex.Message}\n{ex.InnerException?.Message}", ex);
                }
            }
            catch (Exception ex)
            {
                throw new UnitOfWorkTransactionException($"An error occurred while saving changes to the database.\n{ex.Message}\n{ex.InnerException?.Message}", ex);
            }
        }

        public virtual async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (DbUpdateException ex)
            {
                if (ex.GetBaseException() is SqlException sqlException)
                {
                    sqlException.HandleSqlException<UnitOfWorkTransactionException>();
                    throw;
                }
                else
                {
                    throw new UnitOfWorkTransactionException($"An error occurred while saving changes to the database.\n{ex.Message}\n{ex.InnerException?.Message}", ex);
                }
            }
            catch (Exception ex)
            {
                throw new UnitOfWorkTransactionException($"An error occurred while saving changes to the database.\n{ex.Message}\n{ex.InnerException?.Message}", ex);
            }
        }

        #region disposing
        private bool disposed = false; // to detect redundant calls

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~UnitOfWork()
        {
            Dispose(false);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore(bool disposing)
        {
            if (!disposed)
            {
                if (disposing && DbContext != null)
                {
                    // Dispose managed state (managed objects)
                    await DbContext.DisposeAsync();
                }

                disposed = true;
            }
        }

        // Instantiate your resources here
        // For example, SqlConnection, FileStream etc.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    // If you have any managed IDisposable resources, dispose them here.
                    // Managed resources include anything that implements IDisposable
                    // and was instantiated by this class.
                    DbContext?.Dispose();
                }

                // Dispose unmanaged resources.
                // If you have any unmanaged resources, dispose them here.

                disposed = true;
            }
        }
        #endregion
    }
}
