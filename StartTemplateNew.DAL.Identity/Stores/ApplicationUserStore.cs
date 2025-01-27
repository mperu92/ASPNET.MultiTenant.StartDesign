using Microsoft.AspNetCore.Identity;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Repositories.Core;
using StartTemplateNew.DAL.UnitOfWork.Core;
using System.Security.Claims;

namespace StartTemplateNew.DAL.Identity.Stores
{
    public class ApplicationUserStore :
        IUserPasswordStore<UserEntity>,
        IUserLoginStore<UserEntity>,
        IUserClaimStore<UserEntity>,
        IUserRoleStore<UserEntity>,
        IUserEmailStore<UserEntity>,
        IUserPhoneNumberStore<UserEntity>,
        IUserTwoFactorStore<UserEntity>,
        IUserLockoutStore<UserEntity>,
        IQueryableUserStore<UserEntity>,
        IAsyncDisposable
    {
        private readonly IUserRepository _usersRepository;
        private readonly IRoleRepository _rolesRepository;

        public ApplicationUserStore(IUnitOfWork unitOfWork)
        {
            _usersRepository = unitOfWork.GetTenantedRepoImpl<IUserRepository>();
            _rolesRepository = unitOfWork.GetRepoImpl<IRoleRepository>();
        }

        #region disposing
        private bool _disposed = false;

        ~ApplicationUserStore()
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
                    _usersRepository?.Dispose();
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
                if (disposing)
                {
                    if (_usersRepository != null)
                    {
                        await _usersRepository.DisposeAsync().ConfigureAwait(false);
                    }
                    if (_rolesRepository != null)
                    {
                        await _rolesRepository.DisposeAsync().ConfigureAwait(false);
                    }
                }

                _disposed = true;
            }
        }
        #endregion

        public IQueryable<UserEntity> Users
        {
            get
            {
                if (_usersRepository.IsLoggedInWithTenant)
                    return _usersRepository.Query.Where(x => x.TenantId == _usersRepository.TenantId);

                return _usersRepository.Query;
            }
        }

        public async Task<IdentityResult> CreateAsync(UserEntity user, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);

            user.SecurityStamp = Guid.NewGuid().ToString();

            await _usersRepository.AddAsync(user, cancellationToken).ConfigureAwait(false);
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            _usersRepository.Remove(user);
            return Task.FromResult(IdentityResult.Success);
        }

        public async Task<UserEntity?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(userId);

            return await _usersRepository.GetByIdAsync(Guid.Parse(userId), cancellationToken).ConfigureAwait(false);
        }

        public async Task<UserEntity?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(normalizedUserName);

            normalizedUserName = normalizedUserName.ToUpperInvariant();
            return await _usersRepository.GetByFilterAsync(x => normalizedUserName == x.NormalizedUserName, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public Task<string?> GetNormalizedUserNameAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string?> GetUserNameAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.UserName);
        }

        public async Task SetNormalizedUserNameAsync(UserEntity user, string? normalizedName, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(normalizedName))
            {
                ArgumentNullException.ThrowIfNull(user);

                user.NormalizedUserName = normalizedName.ToUpperInvariant();

                await _usersRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task SetUserNameAsync(UserEntity user, string? userName, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                ArgumentNullException.ThrowIfNull(user);

                user.UserName = userName;

                await _usersRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<IdentityResult> UpdateAsync(UserEntity user, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);

            await _usersRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
            return IdentityResult.Success;
        }

        public async Task SetPasswordHashAsync(UserEntity user, string? passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!string.IsNullOrWhiteSpace(passwordHash))
            {
                ArgumentNullException.ThrowIfNull(user);

                user.PasswordHash = passwordHash;
                await _usersRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
            }
        }

        public Task<string?> GetPasswordHashAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.PasswordHash != null);
        }

        public Task AddLoginAsync(UserEntity user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(login);

            _usersRepository.AddLogin(user, login);
            return Task.CompletedTask;
        }

        public Task RemoveLoginAsync(UserEntity user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(loginProvider);
            ArgumentException.ThrowIfNullOrEmpty(providerKey);

            UserLoginInfo login = new(loginProvider, providerKey, "HypeLabUserLogin");
            _usersRepository.RemoveLogin(user, login);
            return Task.CompletedTask;
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            List<UserLoginInfo> logins = user.UserLogins.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, x.ProviderDisplayName)).ToList();
            return Task.FromResult<IList<UserLoginInfo>>(logins);
        }

        public async Task<UserEntity?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentException.ThrowIfNullOrEmpty(loginProvider);
            ArgumentException.ThrowIfNullOrEmpty(providerKey);

            return await _usersRepository.FindByLoginAsync(loginProvider, providerKey).ConfigureAwait(false);
        }

        public Task<IList<Claim>> GetClaimsAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            List<Claim> claims = user.UserClaims.Where(x => !string.IsNullOrEmpty(x.ClaimType) && !string.IsNullOrEmpty(x.ClaimValue)).Select(x => new Claim(x.ClaimType!, x.ClaimValue!)).ToList();
            return Task.FromResult<IList<Claim>>(claims);
        }

        public Task AddClaimsAsync(UserEntity user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (claims.Any())
            {
                ArgumentNullException.ThrowIfNull(user);

                _usersRepository.AddClaims(user, claims);
            }

            return Task.CompletedTask;
        }

        public Task ReplaceClaimAsync(UserEntity user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(claim);
            ArgumentNullException.ThrowIfNull(newClaim);

            _usersRepository.ReplaceClaim(user, claim, newClaim);
            return Task.CompletedTask;
        }

        public Task RemoveClaimsAsync(UserEntity user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (claims.Any())
            {
                ArgumentNullException.ThrowIfNull(user);

                _usersRepository.RemoveClaims(user, claims);
            }

            return Task.CompletedTask;
        }

        public async Task<IList<UserEntity>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(claim);

            return await _usersRepository.GetUsersForClaimAsync(claim, cancellationToken).ConfigureAwait(false);
        }

        public async Task AddToRoleAsync(UserEntity user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrEmpty(roleName);

            string normalizedRoleName = roleName.ToUpperInvariant();
            RoleEntity roleEntity = await _rolesRepository.GetByFilterAsync(a => a.NormalizedName == normalizedRoleName, cancellationToken: cancellationToken)
                ?? throw new InvalidOperationException("Role not found");

            _usersRepository.AddRole(user, roleEntity);
        }

        public async Task RemoveFromRoleAsync(UserEntity user, string roleName, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrEmpty(roleName);

            string normalizedRoleName = roleName.ToUpperInvariant();
            RoleEntity role = await _rolesRepository.GetByFilterAsync(x => x.NormalizedName == normalizedRoleName, cancellationToken: cancellationToken).ConfigureAwait(false)
                ?? throw new InvalidOperationException($"Role with name {roleName} not found");

            _usersRepository.RemoveRole(user, role);
        }

        public async Task<IList<string>> GetRolesAsync(UserEntity user, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);

            if (!await _usersRepository.IsCollectionLoadedAsync(user, "UserRoles", cancellationToken).ConfigureAwait(false))
                await _usersRepository.LoadCollectionAsync(user, "UserRoles", cancellationToken).ConfigureAwait(false);

            return user.UserRoles.Select(x => x.Role?.Name).Where(x => !string.IsNullOrEmpty(x)).ToList()!;
        }

        public async Task<bool> IsInRoleAsync(UserEntity user, string roleName, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrEmpty(roleName);

            return await _usersRepository.IsInRoleAsync(user, roleName, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<UserEntity>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(roleName);

            string normalizedRoleName = roleName.ToUpperInvariant();
            RoleEntity role =
                await _rolesRepository.GetByFilterAsync(x => x.NormalizedName == normalizedRoleName, cancellationToken: cancellationToken).ConfigureAwait(false)
                ?? throw new InvalidOperationException($"Role with name {roleName} not found");

            ICollection<UserEntity> dsl = await _usersRepository.GetAllAsync(x => x.UserRoles.Any(a => a.RoleId == role.Id), cancellationToken: cancellationToken).ConfigureAwait(false);
            return [.. dsl];
        }

        public async Task SetEmailAsync(UserEntity user, string? email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!string.IsNullOrEmpty(email))
            {
                ArgumentNullException.ThrowIfNull(user);

                user.Email = email;
                user.NormalizedEmail = email.ToUpperInvariant();
                await _usersRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
            }
        }

        public Task<string?> GetEmailAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.EmailConfirmed);
        }

        public async Task SetEmailConfirmedAsync(UserEntity user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            user.EmailConfirmed = confirmed;
            await _usersRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
        }

        public async Task<UserEntity?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(normalizedEmail);

            return await _usersRepository.GetByEmailAsync(normalizedEmail, cancellationToken).ConfigureAwait(false);
        }

        public Task<string?> GetNormalizedEmailAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.NormalizedEmail);
        }

        public async Task SetNormalizedEmailAsync(UserEntity user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!string.IsNullOrEmpty(normalizedEmail))
            {
                ArgumentNullException.ThrowIfNull(user);

                user.NormalizedEmail = normalizedEmail.ToUpperInvariant();
                await _usersRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task SetPhoneNumberAsync(UserEntity user, string? phoneNumber, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                ArgumentNullException.ThrowIfNull(user);

                user.PhoneNumber = phoneNumber;
                await _usersRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
            }
        }

        public Task<string?> GetPhoneNumberAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public async Task SetPhoneNumberConfirmedAsync(UserEntity user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            user.PhoneNumberConfirmed = confirmed;
            await _usersRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
        }

        public async Task SetTwoFactorEnabledAsync(UserEntity user, bool enabled, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            user.TwoFactorEnabled = enabled;
            await _usersRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
        }

        public Task<bool> GetTwoFactorEnabledAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.LockoutEnd);
        }

        public async Task SetLockoutEndDateAsync(UserEntity user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            user.LockoutEnd = lockoutEnd;
            await _usersRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> IncrementAccessFailedCountAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            user.AccessFailedCount++;
            await _usersRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
            return user.AccessFailedCount;
        }

        public async Task ResetAccessFailedCountAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            user.AccessFailedCount = 0;
            await _usersRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
        }

        public Task<int> GetAccessFailedCountAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(UserEntity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.LockoutEnabled);
        }

        public async Task SetLockoutEnabledAsync(UserEntity user, bool enabled, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            user.LockoutEnabled = enabled;
            await _usersRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
        }
    }
}
