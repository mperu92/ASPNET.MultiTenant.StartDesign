namespace StartTemplateNew.DAL.TenantUserProvider.Models
{
    public class ClaimUser : IClaimUser
    {
        public ClaimUser(string? id, string? userName, string? tenantId, bool isSysAdmin, bool isTenantAdmin)
        {
            Id = id;
            UserName = userName;
            TenantId = tenantId;
            IsSysAdmin = isSysAdmin;
            IsTenantAdmin = isTenantAdmin;
        }

        public string? Id { get; }
        public string? UserName { get; }
        public bool IsSysAdmin { get; }
        public bool IsTenantAdmin { get; }
        public string? TenantId { get; }
    }
}