using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Services.Core;
using StartTemplateNew.Shared.Services.Models;

namespace StartTemplateNew.Shared.Services.Application
{
    public interface ISetUserProductService : IService
    {
        Task<ServiceResponse> SetUserProductAsync(SetUserProductRequest request, CancellationToken cancellationToken);
    }
}
