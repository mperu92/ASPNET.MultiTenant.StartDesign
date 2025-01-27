using Microsoft.EntityFrameworkCore;
using StartTemplateNew.DAL.Contexts;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Repositories.Core.Base;
using StartTemplateNew.DAL.Repositories.Enums.Queries;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Security.Claims;

namespace StartTemplateNew.DAL.Repositories.Core.Impl
{
    public class RoleRepository : Repository<RoleEntity, Guid>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context)
            : base(context) { }

        private static readonly Expression<Func<RoleEntity, object>> _roleClaimsInclude = x => x.RoleClaims;

        public override async Task<RoleEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await Table
                .Include(_roleClaimsInclude)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                .ConfigureAwait(false);
        }

        public override RoleEntity? GetByFilter(Expression<Func<RoleEntity, bool>>? filter = null, ICollection<Expression<Func<RoleEntity, object>>>? includes = null, TypeReturnBehavior returnBehavior = TypeReturnBehavior.FirstOrDefault)
        {
            if (includes == null)
                includes = [_roleClaimsInclude];
            else if (!includes.Contains(_roleClaimsInclude))
                includes.Add(_roleClaimsInclude);

            return base.GetByFilter(filter, includes, returnBehavior);
        }

        public override async Task<RoleEntity?> GetByFilterAsync(Expression<Func<RoleEntity, bool>>? filter = null, ICollection<Expression<Func<RoleEntity, object>>>? includes = null, TypeReturnBehavior returnBehavior = TypeReturnBehavior.FirstOrDefault, CancellationToken cancellationToken = default)
        {
            if (includes == null)
                includes = [_roleClaimsInclude];
            else if (!includes.Contains(_roleClaimsInclude))
                includes.Add(_roleClaimsInclude);

            return await base.GetByFilterAsync(filter, includes, returnBehavior, cancellationToken).ConfigureAwait(false);
        }

        public IList<Claim> GetClaims(RoleEntity role)
        {
            ArgumentNullException.ThrowIfNull(role);

            EnsureRoleClaimsIsLoaded(role, role.RoleClaims);

            return role.RoleClaims.Where(x => !string.IsNullOrEmpty(x.ClaimType) && !string.IsNullOrEmpty(x.ClaimValue)).Select(x => new Claim(x.ClaimType!, x.ClaimValue!)).ToList();
        }

        public void AddClaims(RoleEntity role, IEnumerable<Claim> claims)
        {
            ArgumentNullException.ThrowIfNull(role);
            ArgumentNullException.ThrowIfNull(claims);

            EnsureRoleClaimsIsLoaded(role, role.RoleClaims);

            foreach (Claim claim in claims)
            {
                role.RoleClaims.Add(new RoleClaimEntity
                {
                    Role = role,
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                });
            }
        }

        public void RemoveClaims(RoleEntity role, IEnumerable<Claim> claims)
        {
            ArgumentNullException.ThrowIfNull(role);
            ArgumentNullException.ThrowIfNull(claims);

            EnsureRoleClaimsIsLoaded(role, role.RoleClaims);

            foreach (Claim claim in claims)
            {
                RoleClaimEntity? roleClaim = role.RoleClaims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
                if (roleClaim != null)
                    role.RoleClaims.Remove(roleClaim);
            }
        }

        public void ReplaceClaim(RoleEntity role, Claim oldClaim, Claim newClaim)
        {
            ArgumentNullException.ThrowIfNull(role);
            ArgumentNullException.ThrowIfNull(oldClaim);
            ArgumentNullException.ThrowIfNull(newClaim);

            EnsureRoleClaimsIsLoaded(role, role.RoleClaims);

            RoleClaimEntity? roleClaim = role.RoleClaims.FirstOrDefault(x => x.ClaimType == oldClaim.Type && x.ClaimValue == oldClaim.Value);
            if (roleClaim != null)
            {
                role.RoleClaims.Remove(roleClaim);
                role.RoleClaims.Add(new RoleClaimEntity
                {
                    Role = role,
                    ClaimType = newClaim.Type,
                    ClaimValue = newClaim.Value
                });
            }
        }

        private void EnsureRoleClaimsIsLoaded(RoleEntity role, [NotNull] ICollection<RoleClaimEntity>? roleClaims)
        {
            if (!_dbContext.Entry(role).Collection(u => u.RoleClaims).IsLoaded)
                Collection(role, "RoleClaims");

            if (roleClaims == null)
                throw new InvalidOperationException("RoleClaims results null after Collection method call");
        }
    }
}
