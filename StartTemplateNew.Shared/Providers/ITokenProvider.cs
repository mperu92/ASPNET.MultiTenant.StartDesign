using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Models.Dto.Responses;

namespace StartTemplateNew.Shared.Providers
{
    public interface ITokenProvider : IProvider
    {
        Task<TokenResponse> GenerateTokenAsync(TokenRequest tokenRequest);
    }
}
