using Microsoft.AspNetCore.Identity;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Repositories.Core;
using StartTemplateNew.DAL.UnitOfWork.Core;
using System.Security.Claims;

namespace StartTemplateNew.DAL.Identity.Stores
{
    public class ApplicationRoleStore : IRoleClaimStore<RoleEntity>, IQueryableRoleStore<RoleEntity>, IAsyncDisposable
    {
        private readonly IRoleRepository _rolesRepository;

        public ApplicationRoleStore(IUnitOfWork unitOfWork)
        {
            _rolesRepository = unitOfWork.GetRepoImpl<IRoleRepository>();
        }

        #region disposing
        private bool _disposed = false;

        ~ApplicationRoleStore()
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
                    _rolesRepository?.Dispose();
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
                if (disposing && _rolesRepository != null)
                {
                    await _rolesRepository.DisposeAsync().ConfigureAwait(false);
                }

                _disposed = true;
            }
        }
        #endregion

        public IQueryable<RoleEntity> Roles
        {
            get
            {
                return _rolesRepository.Query;
            }
        }

        public async Task AddClaimAsync(RoleEntity role, Claim claim, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(role);
            ArgumentNullException.ThrowIfNull(claim);

            // Verifica se la collection RoleClaims è null
            if (!await _rolesRepository.IsCollectionLoadedAsync(role, "RoleClaims", cancellationToken).ConfigureAwait(false))
                await _rolesRepository.LoadCollectionAsync(role, "RoleClaims", cancellationToken).ConfigureAwait(false);

            // Aggiungi il nuovo claim alla collection RoleClaims
            role.RoleClaims.Add(new RoleClaimEntity
            {
                RoleId = role.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            });

            // Aggiorna il ruolo nel contesto di persistenza
            await _rolesRepository.UpdateAsync(role, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IdentityResult> CreateAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(role);

            await _rolesRepository.AddAsync(role).ConfigureAwait(false);

            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(role);

            _rolesRepository.Remove(role);

            return Task.FromResult(IdentityResult.Success);
        }

        public async Task<RoleEntity?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(roleId);

            return await _rolesRepository.GetByIdAsync(new Guid(roleId), cancellationToken).ConfigureAwait(false);
        }

        public async Task<RoleEntity?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(normalizedRoleName);

            return await _rolesRepository.GetByFilterAsync(x => x.NormalizedName == normalizedRoleName, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<Claim>> GetClaimsAsync(RoleEntity role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(role);

            if (!await _rolesRepository.IsCollectionLoadedAsync(role, "RoleClaims", cancellationToken).ConfigureAwait(false))
                await _rolesRepository.LoadCollectionAsync(role, "RoleClaims", cancellationToken).ConfigureAwait(false);

            if (role.RoleClaims.Count > 0)
            {
                return role.RoleClaims
                    .Where(x => !string.IsNullOrEmpty(x.ClaimType) && !string.IsNullOrEmpty(x.ClaimValue))
                    .Select(x => new Claim(x.ClaimType!, x.ClaimValue!))
                    .ToList();
            }

            return [];
        }

        public Task<string?> GetNormalizedRoleNameAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(role);

            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(role);

            return Task.FromResult(role.Id.ToString());
        }

        public Task<string?> GetRoleNameAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(role);

            return Task.FromResult(role.Name);
        }

        public async Task RemoveClaimAsync(RoleEntity role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(role);
            ArgumentNullException.ThrowIfNull(claim);

            if (!await _rolesRepository.IsCollectionLoadedAsync(role, "RoleClaims", cancellationToken).ConfigureAwait(false))
                await _rolesRepository.LoadCollectionAsync(role, "RoleClaims", cancellationToken).ConfigureAwait(false);

            RoleClaimEntity? roleClaim = role.RoleClaims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            if (roleClaim != null)
                role.RoleClaims.Remove(roleClaim);
        }

        public Task SetNormalizedRoleNameAsync(RoleEntity role, string? normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrEmpty(normalizedName))
                return Task.CompletedTask;

            ArgumentNullException.ThrowIfNull(role);

            role.NormalizedName = normalizedName;
            if (!string.Equals(role.Name, role.NormalizedName, StringComparison.OrdinalIgnoreCase))
                role.Name = role.NormalizedName;

            _rolesRepository.Update(role);

            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(RoleEntity role, string? roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrEmpty(roleName))
                return Task.CompletedTask;

            ArgumentNullException.ThrowIfNull(role);

            role.Name = roleName;
            if (!string.Equals(role.Name, role.NormalizedName, StringComparison.OrdinalIgnoreCase))
                role.NormalizedName = role.Name.ToUpperInvariant();

            _rolesRepository.Update(role);

            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(role);

            _rolesRepository.Update(role);

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
