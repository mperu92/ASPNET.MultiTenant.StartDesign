using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using StartTemplateNew.Shared.Models.Dto.Requests;

namespace StartTemplateNew.Shared.Helpers.Binders.Models
{
    public class TokenRequestModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            IFormCollection form = bindingContext.HttpContext.Request.Form;

            if (!form.TryGetValue("grant_type", out StringValues grantType))
            {
                bindingContext.ModelState.AddModelError("grant_type", "The grant_type field is required.");
                return Task.CompletedTask;
            }

            if (!form.TryGetValue("client_id", out StringValues clientId))
            {
                bindingContext.ModelState.AddModelError("client_id", "The client_id field is required.");
                return Task.CompletedTask;
            }

            if (!form.TryGetValue("client_secret", out StringValues clientSecret))
            {
                bindingContext.ModelState.AddModelError("client_secret", "The client_secret field is required.");
                return Task.CompletedTask;
            }

            _ = form.TryGetValue("refresh_token", out StringValues refreshToken);
            _ = form.TryGetValue("code", out StringValues code);
            _ = form.TryGetValue("redirect_uri", out StringValues redirectUri);

            TokenRequest model = new()
            {
                GrantType = grantType.ToString(),
                Code = code,
                RedirectUri = redirectUri,
                ClientId = clientId.ToString(),
                ClientSecret = clientSecret.ToString(),
                RefreshToken = refreshToken,
            };

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}
