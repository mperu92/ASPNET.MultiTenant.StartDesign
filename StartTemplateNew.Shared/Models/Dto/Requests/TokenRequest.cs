using Microsoft.AspNetCore.Mvc;
using StartTemplateNew.Shared.Helpers.Binders.Models;

namespace StartTemplateNew.Shared.Models.Dto.Requests
{
    [ModelBinder(BinderType = typeof(TokenRequestModelBinder))]
    public class TokenRequest
    {
        public string GrantType { get; set; } = default!;
        public string? Code { get; set; }
        public string? RedirectUri { get; set; }
        public string ClientId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
        public string? RefreshToken { get; set; }
        public string? Username { get; set; } // Aggiungi per il grant type password
        public string? Password { get; set; } // Aggiungi per il grant type password
    }
}
