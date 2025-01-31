using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.Shared.Models.Dto.Requests
{
    public interface ICreateUpdateUserRequest
    {
        Guid Id { get; set; }
        Guid? RoleId { get; set; }
        string Password { get; set; }
    }

    public class CreateUpdateUserRequest : CreateUpdateUserFormRequest, ICreateUpdateUserRequest
    {
        [SetsRequiredMembers]
        public CreateUpdateUserRequest(string userName, string email, string password, string firstName, string lastName)
            : base(userName, email, password, firstName, lastName) { }

        public Guid? RoleId { get; set; }
    }

    public class CreateUpdateUserWithTenantRequest : CreateUpdateUserRequest
    {
        [SetsRequiredMembers]
        public CreateUpdateUserWithTenantRequest(string userName, string email, string password, string firstName, string lastName)
            : base(userName, email, password, firstName, lastName) { }

        public Guid? TenantId { get; set; }
    }
}
