using Microsoft.AspNetCore.Identity;
using StartTemplateNew.DAL.Entities;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Repositories.Core.Base;
using System.Security.Claims;

namespace StartTemplateNew.DAL.Repositories.Core
{
    public interface IUserRepository : ITenantedRepository<UserEntity, Guid, TenantEntity, Guid, UserEntity, Guid>
    {
        public Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        void AddClaims(UserEntity user, IEnumerable<Claim> claims);
        void RemoveClaims(UserEntity user, IEnumerable<Claim> claims);
        void ReplaceClaim(UserEntity user, Claim oldClaim, Claim newClaim);
        void AddLogin(UserEntity user, UserLoginInfo login);
        void RemoveLogin(UserEntity user, UserLoginInfo login);
        void AddRole(UserEntity user, RoleEntity role);
        void RemoveRole(UserEntity user, RoleEntity role);
        Task<bool> IsInRoleAsync(UserEntity user, string roleName, CancellationToken cancellationToken = default);
        Task<UserEntity?> FindByLoginAsync(string loginProvider, string providerKey);
        Task<IList<UserEntity>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default);
    }
}
