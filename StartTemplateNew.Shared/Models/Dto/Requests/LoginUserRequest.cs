using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.Shared.Models.Dto.Requests
{
    public class LoginUserRequest
    {
        [SetsRequiredMembers]
        public LoginUserRequest(string userName, string password, bool rememberMe = false)
        {
            Username = userName;
            Password = password;
            RememberMe = rememberMe;
        }
        public required string Username { get; set; }
        public required string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
