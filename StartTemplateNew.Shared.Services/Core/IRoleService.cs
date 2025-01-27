using StartTemplateNew.Shared.Models.Dto.Identity;
using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Services.Models;

namespace StartTemplateNew.Shared.Services.Core
{
    public interface IRoleService : IService
    {
        Task<ServiceResponse<ICollection<Role>>> GetRolesAsync(CancellationToken cancellationToken = default);
        Task<ServiceResponse> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default);
        Task<ServiceResponse> DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
    }
}
