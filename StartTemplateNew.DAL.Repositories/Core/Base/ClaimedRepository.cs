using StartTemplateNew.DAL.Contexts;
using StartTemplateNew.DAL.Entities.Auditing;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Repositories.Helpers.Tenant;
using StartTemplateNew.DAL.TenantUserProvider.Core;
using StartTemplateNew.DAL.TenantUserProvider.Helpers;
using StartTemplateNew.DAL.TenantUserProvider.Models;
using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.DAL.Repositories.Core.Base
{
    public class ClaimedRepository<TEntity, TKey, TClaimUser, TClaimUserKey> : Repository<TEntity, TKey>, IClaimedRepository<TEntity, TKey, TClaimUser, TClaimUserKey>
        where TEntity : class, IKeyedEntity<TKey>, IAuditedEntity
        where TKey : struct, IEquatable<TKey>
        where TClaimUser : class, IKeyedEntity<TClaimUserKey>
        where TClaimUserKey : struct, IEquatable<TClaimUserKey>
    {
        private readonly ITheTypeConverter<TClaimUserKey> _converter;

        private readonly Lazy<IClaimUser> _lazyClaimUser;

        /// <summary>
        /// IClaimUser è immutabile, quindi è sicuro restituire il valore direttamente
        /// </summary>
        public IClaimUser ClaimUser => _lazyClaimUser.Value;

        /// <summary>
        /// TClaimUser è mutabile, ma in un contesto dove è registrato tutto scoped, è sicuro restituire il valore direttamente se non ci accedi tramite operazioni parallele
        /// </summary>
        private TClaimUser? _user;
        public TClaimUser? User
        {
            get
            {
                if (_user == null && !string.IsNullOrEmpty(ClaimUser.Id))
                    _user = _dbContext.Set<TClaimUser>().Find(_converter.ConvertFromInvariantString_n(ClaimUser.Id));

                return _user;
            }

            private set
            {
                _user = value;
            }
        }

        [MemberNotNullWhen(true, nameof(User))]
        public bool IsLoggedIn
        {
            get
            {
                if (!string.IsNullOrEmpty(ClaimUser.Id))
                    return User != null;

                return false;
            }
        }

        [MemberNotNullWhen(true, nameof(User))]
        public bool IsLoggedInWithTenant => IsLoggedIn && !string.IsNullOrEmpty(ClaimUser.TenantId);

        [SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out")]
        private static IClaimUser InitializeClaimUser(IPrincipalProvider principalProvider)
        {
            //if (User == null && !string.IsNullOrEmpty(claimUser.Id))
            //{
            //    User = _dbContext.Set<TClaimUser>().Find(_converter.Convert(claimUser.Id));
            //}

            return principalProvider.ProvideUser();
        }

        public ClaimedRepository(ApplicationDbContext dbContext, IPrincipalProvider principalProvider, ITheTypeConverter<TClaimUserKey> converter)
            : base(dbContext)
        {
            _lazyClaimUser = new Lazy<IClaimUser>(() => ClaimedRepository<TEntity, TKey, TClaimUser, TClaimUserKey>.InitializeClaimUser(principalProvider));
            _converter = converter;
        }

        public override void Add(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            entity.HandleAuditing<TEntity, TKey, TClaimUser, TClaimUserKey>(User, IsLoggedIn);
            Table.Add(entity);
        }

        public async override Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await entity.HandleAuditing<TEntity, TKey, TClaimUser, TClaimUserKey>(User, IsLoggedIn).ConfigureAwait(false);
            await Table.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        }

        public override void AddRange(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (TEntity entity in entities)
            {
                entity.HandleAuditing<TEntity, TKey, TClaimUser, TClaimUserKey>(User, IsLoggedIn);
            }

            Table.AddRange(entities);
        }

        public override void Update(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.HandleAuditing<TEntity, TKey, TClaimUser, TClaimUserKey>(User, IsLoggedIn);

            Table.Update(entity);
        }

        public async override Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentNullException.ThrowIfNull(entity);

            await entity.HandleAuditing<TEntity, TKey, TClaimUser, TClaimUserKey>(User, IsLoggedIn).ConfigureAwait(false);
            Table.Update(entity);
        }

        public override void UpdateRange(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (TEntity entity in entities)
            {
                entity.HandleAuditing<TEntity, TKey, TClaimUser, TClaimUserKey>(User, IsLoggedIn);
            }

            Table.UpdateRange(entities);
        }
    }
}
