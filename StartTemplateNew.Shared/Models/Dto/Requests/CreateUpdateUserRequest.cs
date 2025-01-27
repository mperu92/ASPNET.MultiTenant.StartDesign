using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.Shared.Models.Dto.Requests
{
    public class CreateUpdateUserRequest : CreateUpdateUserFormRequest
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
