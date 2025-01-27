using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Repositories.Core.Base;
using System.Security.Claims;

namespace StartTemplateNew.DAL.Repositories.Core
{
    public interface IRoleRepository : IRepository<RoleEntity, Guid>
    {
        IList<Claim> GetClaims(RoleEntity role);
        void AddClaims(RoleEntity role, IEnumerable<Claim> claims);
        void RemoveClaims(RoleEntity role, IEnumerable<Claim> claims);
        void ReplaceClaim(RoleEntity role, Claim oldClaim, Claim newClaim);
    }
}
