namespace StartTemplateNew.DAL.TenantUserProvider.Models
{
    /// <summary>
    /// Represents a user with claims. Should be an immutable type.
    /// </summary>
    public interface IClaimUser
    {
        string? Id { get; }
        string? UserName { get; }
        bool IsSysAdmin { get; }
        bool IsTenantAdmin { get; }
        string? TenantId { get; }
    }
}
