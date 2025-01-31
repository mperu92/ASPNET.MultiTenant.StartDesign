using StartTemplateNew.Shared.Models.Dto;
using StartTemplateNew.Shared.Models.Dto.Identity;
using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Services.Core;
using StartTemplateNew.Shared.Services.Models;

namespace StartTemplateNew.Shared.Services.Domain
{
    public interface IUserService : IService
    {
        Task<ServiceResponse<ICollection<User>>> GetUsersAsync(GetUsersRequest request, CancellationToken cancellationToken = default);
        Task<ServiceResponse> CreateUserFormAsync(CreateUpdateUserFormRequest request, CancellationToken cancellationToken = default);
        Task<ServiceResponse<EntityStateInfo>> CreateUpdateUserAsync(CreateUpdateUserRequest request, CancellationToken cancellationToken = default);
        Task<ServiceResponse<EntityStateInfo>> CreateUpdateUserAsync(ICreateUpdateUserRequest requestInt, CancellationToken cancellationToken = default);
        Task<ServiceResponse<EntityStateInfo>> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<ServiceResponse> SignInAsync(string userName, string password, bool rememberMe, CancellationToken cancellationToken = default);
        Task<ServiceResponse> SignOutAsync(CancellationToken cancellationToken = default);
    }
}
