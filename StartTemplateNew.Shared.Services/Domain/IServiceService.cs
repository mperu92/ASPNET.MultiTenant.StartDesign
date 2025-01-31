using StartTemplateNew.Shared.Models.Dto;
using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Models.Dto.Services;
using StartTemplateNew.Shared.Services.Core;
using StartTemplateNew.Shared.Services.Models;

namespace StartTemplateNew.Shared.Services.Domain
{
    public interface IServiceService : IService
    {
        Task<ServiceResponse<ICollection<Service>>> GetServicesAsync(CancellationToken cancellationToken = default);
        Task<ServiceResponse<EntityStateInfo>> CreateUpdateServiceAsync(CreateUpdateServiceRequest request, CancellationToken cancellationToken = default);
        Task<ServiceResponse<EntityStateInfo>> DeleteServiceAsync(Guid serviceId, CancellationToken cancellationToken = default);
        Task<ServiceResponse<EntityStateInfo>> SetTenantServiceAsync(SetTenantServiceRequest request, CancellationToken cancellationToken = default);
    }
}
