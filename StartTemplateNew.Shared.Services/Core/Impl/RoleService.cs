using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Repositories.Core;
using StartTemplateNew.DAL.UnitOfWork.Core;
using StartTemplateNew.Shared.Helpers;
using StartTemplateNew.Shared.Helpers.Extensions;
using StartTemplateNew.Shared.Models.Dto.Identity;
using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Services.Models;

namespace StartTemplateNew.Shared.Services.Core.Impl
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<RoleEntity> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, RoleManager<RoleEntity> roleManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ICollection<Role>>> GetRolesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                ICollection<RoleEntity> roles = await _roleManager.Roles.ToListAsync(cancellationToken).ConfigureAwait(false);
                return ServiceResponse<ICollection<Role>>.Success(_mapper.Map<ICollection<Role>>(roles));
            }
            catch (Exception ex)
            {
                return ServiceResponse<ICollection<Role>>.Error($"Error getting roles.\n{ex.GetFullMessage()}");
            }
        }

        public async Task<ServiceResponse> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                RoleEntity role = _mapper.Map<RoleEntity>(request);
                IdentityResult result = await _roleManager.CreateAsync(role).ConfigureAwait(false);
                if (!result.Succeeded)
                    return ServiceResponse.Error($"Error creating role.\n{result.GetErrors()}");

                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
                return ServiceResponse.Success("Role created successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse.Error($"Error creating role.\n{ex.GetFullMessage()}");
            }
        }

        public async Task<ServiceResponse> DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                RoleEntity? role = await _roleManager.FindByIdAsync(roleId.ToString()).ConfigureAwait(false);
                if (role is null)
                    return ServiceResponse.Error("Role not found.");
                IdentityResult result = await _roleManager.DeleteAsync(role).ConfigureAwait(false);
                if (result.Succeeded)
                    return ServiceResponse.Success("Role deleted successfully.");

                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

                return ServiceResponse.Error($"Error deleting role.\n{result.GetErrors()}");
            }
            catch (Exception ex)
            {
                return ServiceResponse.Error($"Error deleting role.\n{ex.GetFullMessage()}");
            }
        }
    }
}
