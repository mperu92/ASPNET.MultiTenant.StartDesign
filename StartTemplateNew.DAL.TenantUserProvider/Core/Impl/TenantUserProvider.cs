using Microsoft.AspNetCore.Http;
using StartTemplateNew.DAL.TenantUserProvider.Models;

namespace StartTemplateNew.DAL.TenantUserProvider.Core.Impl
{
    public class TenantUserProvider : PrincipalProvider, ITenantUserProvider
    {
        public TenantUserProvider(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor) { }
    }
}
