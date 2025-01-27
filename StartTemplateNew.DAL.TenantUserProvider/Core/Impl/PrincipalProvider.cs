using Microsoft.AspNetCore.Http;
using StartTemplateNew.DAL.TenantUserProvider.Helpers.Const;
using StartTemplateNew.DAL.TenantUserProvider.Models;
using System.Security.Claims;

namespace StartTemplateNew.DAL.TenantUserProvider.Core.Impl
{
    public class PrincipalProvider : IPrincipalProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PrincipalProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IClaimUser ProvideUser()
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated ?? false)
            {
                IEnumerable<Claim> claims = httpContext.User.Claims;

                string? id = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                string? userName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                string? tenantId = claims.FirstOrDefault(x => x.Type == UserTenantClaimTypes.TenantId)?.Value;
                string? isSysAdmin = claims.FirstOrDefault(x => x.Type == UserTenantClaimTypes.SysAdmin)?.Value;
                string? isTenantAdmin = claims.FirstOrDefault(x => x.Type == UserTenantClaimTypes.TenantAdmin)?.Value;
                bool isSysAdminBool = !string.IsNullOrEmpty(isSysAdmin) && bool.TryParse(isSysAdmin, out bool sysAdmin) && sysAdmin;
                bool isTenantAdminBool = !string.IsNullOrEmpty(isTenantAdmin) && bool.TryParse(isTenantAdmin, out bool tenantAdmin) && tenantAdmin;

                return new ClaimUser(id, userName, tenantId, isSysAdminBool, isTenantAdminBool);
            }

            return new ClaimUser(null, null, null, false, false);
        }
    }
}
