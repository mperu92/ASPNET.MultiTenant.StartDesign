using StartTemplateNew.Shared.Models.Dto.Base.Requests;
using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.Shared.Models.Dto.Requests
{
    public class CreateUpdateUserFormRequest : BaseCommandRequest<Guid>
    {
        [SetsRequiredMembers]
        public CreateUpdateUserFormRequest(string userName, string email, string password, string firstName, string lastName)
        {
            UserName = userName;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }

        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
