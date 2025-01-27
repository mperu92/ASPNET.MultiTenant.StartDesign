using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StartTemplateNew.DAL.Contexts;
using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Repositories.Core.Base;
using StartTemplateNew.DAL.Repositories.Enums.Queries;
using StartTemplateNew.DAL.TenantUserProvider.Core;
using StartTemplateNew.DAL.TenantUserProvider.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Security.Claims;

namespace StartTemplateNew.DAL.Repositories.Core.Impl
{
    public class UserRepository : TenantedRepository<UserEntity, Guid, TenantEntity, Guid, UserEntity, Guid>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context, IPrincipalProvider principalProvider, ITheTypeConverter<Guid> parentConverter, ITheTypeConverter<Guid> childConverter)
         : base(context, principalProvider, parentConverter, childConverter) { }

        private static readonly Expression<Func<UserEntity, object>> _userClaimsInclude = x => x.UserClaims;
        private static readonly Expression<Func<UserEntity, object>> _userLoginsInclude = x => x.UserLogins;
        private static readonly Expression<Func<UserEntity, object>> _userRolesInclude = x => x.UserRoles;
        private static readonly Expression<Func<UserEntity, object>> _userTokensInclude = x => x.UserTokens;

        private static readonly List<Expression<Func<UserEntity, object>>> _identityUserIncludes = new()
        {
            _userClaimsInclude,
            _userLoginsInclude,
            _userRolesInclude,
            _userTokensInclude
        };

        public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(email);

            string normalizedEmail = email.ToUpperInvariant();

            return await Table
                .Include(_userClaimsInclude)
                .Include(_userLoginsInclude)
                .Include(_userRolesInclude)
                .Include(_userTokensInclude)
                .FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken).ConfigureAwait(false);
        }

        public override async Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await Table
                .Include(_userClaimsInclude)
                .Include(_userLoginsInclude)
                .Include(_userRolesInclude)
                .Include(_userTokensInclude)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
        }

        public override UserEntity? GetByFilter(Expression<Func<UserEntity, bool>>? filter = null, ICollection<Expression<Func<UserEntity, object>>>? includes = null, TypeReturnBehavior returnBehavior = TypeReturnBehavior.FirstOrDefault)
        {
            return base.GetByFilter(filter ?? _noFilter, HandleIncludes(includes), returnBehavior);
        }

        public override async Task<UserEntity?> GetByFilterAsync(Expression<Func<UserEntity, bool>>? filter = null, ICollection<Expression<Func<UserEntity, object>>>? includes = null, TypeReturnBehavior returnBehavior = TypeReturnBehavior.FirstOrDefault, CancellationToken cancellationToken = default)
        {
            return await base.GetByFilterAsync(filter ?? _noFilter, HandleIncludes(includes), returnBehavior, cancellationToken).ConfigureAwait(false);
        }

        public void AddClaims(UserEntity user, IEnumerable<Claim> claims)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(claims);

            EnsureUserClaimsIsLoaded(user, user.UserClaims);

            foreach (Claim claim in claims)
            {
                user.UserClaims.Add(new UserClaimEntity
                {
                    User = user,
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                });
            }
        }

        public void RemoveClaims(UserEntity user, IEnumerable<Claim> claims)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(claims);

            EnsureUserClaimsIsLoaded(user, user.UserClaims);

            foreach (Claim claim in claims)
            {
                UserClaimEntity? userClaim = user.UserClaims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
                if (userClaim != null)
                    user.UserClaims.Remove(userClaim);
            }
        }

        public void ReplaceClaim(UserEntity user, Claim oldClaim, Claim newClaim)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(oldClaim);
            ArgumentNullException.ThrowIfNull(newClaim);

            EnsureUserClaimsIsLoaded(user, user.UserClaims);

            UserClaimEntity? userClaim = user.UserClaims.FirstOrDefault(x => x.ClaimType == oldClaim.Type && x.ClaimValue == oldClaim.Value);
            if (userClaim != null)
            {
                user.UserClaims.Remove(userClaim);
                user.UserClaims.Add(new UserClaimEntity
                {
                    User = user,
                    ClaimType = newClaim.Type,
                    ClaimValue = newClaim.Value
                });
            }
        }

        public void AddLogin(UserEntity user, UserLoginInfo login)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(login);

            EnsureUserLoginsIsLoaded(user, user.UserLogins);

            user.UserLogins.Add(new UserLoginEntity
            {
                User = user,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                ProviderDisplayName = login.ProviderDisplayName
            });
        }

        public void RemoveLogin(UserEntity user, UserLoginInfo login)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(login);

            EnsureUserLoginsIsLoaded(user, user.UserLogins);

            UserLoginEntity? userLogin = user.UserLogins.FirstOrDefault(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);
            if (userLogin != null)
                user.UserLogins.Remove(userLogin);
        }

        public void AddRole(UserEntity user, RoleEntity role)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(role);

            EnsureUserRolesIsLoaded(user, user.UserRoles);

            user.UserRoles.Add(new UserRoleEntity
            {
                User = user,
                Role = role
            });
        }

        public void RemoveRole(UserEntity user, RoleEntity role)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(role);

            EnsureUserRolesIsLoaded(user, user.UserRoles);

            UserRoleEntity? userRole = user.UserRoles.FirstOrDefault(x => x.RoleId == role.Id);
            if (userRole != null)
                user.UserRoles.Remove(userRole);
        }

        public async Task<bool> IsInRoleAsync(UserEntity user, string roleName, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrWhiteSpace(roleName);

            string normalizedRoleName = roleName.ToUpperInvariant();

            return await Table
                .Where(x => x.Id == user.Id)
                .AnyAsync(x => x.UserRoles.Any(r => r.Role != null && r.Role.NormalizedName == normalizedRoleName), cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<UserEntity?> FindByLoginAsync(string loginProvider, string providerKey)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(loginProvider);
            ArgumentException.ThrowIfNullOrWhiteSpace(providerKey);

            return await Table
                .Include(_userClaimsInclude)
                .Include(_userLoginsInclude)
                .Include(_userRolesInclude)
                .Include(_userTokensInclude)
                .FirstOrDefaultAsync(x => x.UserLogins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey)).ConfigureAwait(false);
        }

        public async Task<IList<UserEntity>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(claim);

            return await Table
                .Include(_userClaimsInclude)
                .Include(_userLoginsInclude)
                .Include(_userRolesInclude)
                .Include(_userTokensInclude)
                .Where(x => x.UserClaims.Any(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        private void EnsureUserClaimsIsLoaded(UserEntity user, [NotNull] ICollection<UserClaimEntity>? userClaims)
        {
            if (!_dbContext.Entry(user).Collection(u => u.UserClaims).IsLoaded)
                Collection(user, "UserClaims");

            if (userClaims == null)
                throw new InvalidOperationException("UserClaims results null after Collection method call");
        }

        private void EnsureUserLoginsIsLoaded(UserEntity user, [NotNull] ICollection<UserLoginEntity>? userLogins)
        {
            if (!_dbContext.Entry(user).Collection(u => u.UserLogins).IsLoaded)
                Collection(user, "UserLogins");

            if (userLogins == null)
                throw new InvalidOperationException("UserLogins results null after Collection method call");
        }

        private void EnsureUserRolesIsLoaded(UserEntity user, [NotNull] ICollection<UserRoleEntity>? userRoles)
        {
            if (!_dbContext.Entry(user).Collection(u => u.UserRoles).IsLoaded)
                Collection(user, "UserRoles");

            if (userRoles == null)
                throw new InvalidOperationException("UserRoles results null after Collection method call");
        }

        private static ICollection<Expression<Func<UserEntity, object>>>? HandleIncludes(ICollection<Expression<Func<UserEntity, object>>>? includes)
        {
            if (includes == null)
            {
                includes = [.. _identityUserIncludes];
            }
            else
            {
                if (!includes.Contains(_userClaimsInclude))
                    includes.Add(_userClaimsInclude);
                if (!includes.Contains(_userLoginsInclude))
                    includes.Add(_userLoginsInclude);
                if (!includes.Contains(_userRolesInclude))
                    includes.Add(_userRolesInclude);
                if (!includes.Contains(_userTokensInclude))
                    includes.Add(_userTokensInclude);
            }

            return includes;
        }
    }
}
