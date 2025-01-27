using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Models.Dto.Responses;
using StartTemplateNew.Shared.Services.Models;

namespace StartTemplateNew.Shared.Services.Core
{
    public interface ITokenService : IService
    {
        Task<ServiceResponse<TokenResponse>> GenerateTokenAsync(TokenRequest request);
    }
}
