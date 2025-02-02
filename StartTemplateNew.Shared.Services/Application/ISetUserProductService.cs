using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Services.Core;
using StartTemplateNew.Shared.Services.Models;

namespace StartTemplateNew.Shared.Services.Application
{
    public interface ISetUserProductService : IService
    {
        /// <summary>
        /// Implements ROP pattern to set a product for a user, ensuring that always a response is returned.
        /// </summary>
        /// <param name="request">The client json request</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>A dataless service response that notices what occurred</returns>
        Task<ServiceResponse> SetUserProductAsync(SetUserProductRequest request, CancellationToken cancellationToken);
    }
}
