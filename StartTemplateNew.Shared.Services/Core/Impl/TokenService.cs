using StartTemplateNew.Shared.Exceptions;
using StartTemplateNew.Shared.Helpers.Extensions;
using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Models.Dto.Responses;
using StartTemplateNew.Shared.Providers;
using StartTemplateNew.Shared.Services.Models;
using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.Shared.Services.Core.Impl
{
    [SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "<Pending>")]
    internal class TokenService(ITokenProvider tokenProvider) : ITokenService
    {
        private readonly ITokenProvider _tokenProvider = tokenProvider;

        public async Task<ServiceResponse<TokenResponse>> GenerateTokenAsync(TokenRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentException.ThrowIfNullOrEmpty(request.ClientId);
            ArgumentException.ThrowIfNullOrEmpty(request.ClientSecret);
            ArgumentException.ThrowIfNullOrEmpty(request.GrantType);

            //Client? client = AuthorizedClients.Clients.Find(c => c.ClientId == request.ClientId && c.ClientSecret == request.ClientSecret);
            //if (client == null)
            //    return ServiceResponse<TokenResponse>.Error("Invalid client credentials.");

            try
            {
                TokenResponse resp = await _tokenProvider.GenerateTokenAsync(request).ConfigureAwait(false);

                return ServiceResponse<TokenResponse>.Success(resp);
            }
            catch (TokenGenerationException ex)
            {
                return ServiceResponse<TokenResponse>.Error(ex.GetFullMessage());
            }
            catch (Exception ex)
            {
                return ServiceResponse<TokenResponse>.Error($"Error during token generation.\n{ex.GetFullMessage()}");
            }
        }
    }
}
