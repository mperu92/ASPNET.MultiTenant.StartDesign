using StartTemplateNew.DAL.TenantUserProvider.Models;

namespace StartTemplateNew.DAL.TenantUserProvider.Core
{
    public interface IPrincipalProvider
    {
        IClaimUser ProvideUser();
    }
}
