using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StartTemplateNew.DAL.Entities.Identity;
using StartTemplateNew.DAL.Repositories.Core;
using StartTemplateNew.DAL.Repositories.Helpers;
using StartTemplateNew.DAL.Repositories.Models;
using StartTemplateNew.DAL.TenantUserProvider.Helpers.Const;
using StartTemplateNew.DAL.UnitOfWork.Core;
using StartTemplateNew.Shared.Enums;
using StartTemplateNew.Shared.Helpers;
using StartTemplateNew.Shared.Helpers.Extensions;
using StartTemplateNew.Shared.Models.Dto;
using StartTemplateNew.Shared.Models.Dto.Base.Requests;
using StartTemplateNew.Shared.Models.Dto.Identity;
using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Services.Models;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Security.Claims;

namespace StartTemplateNew.Shared.Services.Core.Impl
{
    [SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out")]
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<RoleEntity> _roleManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager, SignInManager<UserEntity> signInManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = unitOfWork.GetTenantedRepoImpl<IUserRepository>();
        }

        public async Task<ServiceResponse<ICollection<User>>> GetUsersAsync(GetUsersRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                Expression<Func<UserEntity, bool>>? filter = null;
                if (request.HasSearchFilters)
                {
                    foreach (RequestSearchField field in request.SearchFields)
                    {
                        Expression<Func<UserEntity, bool>> filterExpression = FilteringHelper<UserEntity>.CreateExpressionFromSearchField(field);
                        filter = filter is null ? filterExpression : filter.AndAlso(filterExpression);
                    }
                }

                QueryOrderable<UserEntity>? orderable = null;
                if (request.HasOrder)
                {
                    Expression<Func<UserEntity, object>> orderExpr = OrderingHelper<UserEntity>.CreateExpressionFromOrderFieldString(request.OrderField);
                    orderable = new(orderExpr, request.IsAscending);
                }

                //ICollection<Expression<Func<UserEntity, object>>> includes =
                //[
                //    x => x.UserRoles,
                //];

                ICollection<string> includesStrings =
                [
                    "UserRoles",
                    "UserRoles.Role"
                ];

                QueryTotalCountPair<UserEntity> resp = await _userRepository
                    .GetAllWithCountAsync(filter: filter, pageIndex: request.Pagination.Page, pageSize: request.Pagination.PageSize, includes: includesStrings, queryOrderable: orderable, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                IQueryable<UserEntity> usersQuery = resp.Query;
                ICollection<UserEntity> users = await usersQuery.ToListAsync(cancellationToken).ConfigureAwait(false);

                return ServiceResponse<ICollection<User>>.Success(_mapper.Map<ICollection<User>>(users), request.Pagination.SetTotalCount(resp.TotalCount));
            }
            catch (Exception ex)
            {
                return ServiceResponse<ICollection<User>>.Error($"Error getting users.\n{ex.GetFullMessage()}");
            }
        }

        public async Task<ServiceResponse> CreateUserFormAsync(CreateUpdateUserFormRequest request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                UserEntity user = _mapper.Map<UserEntity>(request);
                IdentityResult result = await _userManager.CreateAsync(user, request.Password).ConfigureAwait(false);
                if (!result.Succeeded)
                    return ServiceResponse.Error($"Error creating user from form.\n{result.GetErrors()}");

                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

                return ServiceResponse.Success($"User {user.UserName} created successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse.Error($"Error creating user from form.\n{ex.GetFullMessage()}");
            }
        }

        public async Task<ServiceResponse<EntityStateInfo>> CreateUpdateUserAsync(CreateUpdateUserRequest request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                UserEntity user = _mapper.Map<UserEntity>(request);

                EntityStateInfo entityStateInfo;
                if (request.Id != Guid.Empty)
                {
                    entityStateInfo = await UpdateUserAsync(user, cancellationToken).ConfigureAwait(false);
                    if (!entityStateInfo.Succeeded)
                        return ServiceResponse<EntityStateInfo>.Error(entityStateInfo.Message);
                }
                else
                {
                    entityStateInfo = await CreateUserAsync(user, request.Password, cancellationToken).ConfigureAwait(false);
                    if (!entityStateInfo.Succeeded)
                        return ServiceResponse<EntityStateInfo>.Error(entityStateInfo.Message);
                }

                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

                if (request.RoleId.HasValue)
                {
                    RoleEntity? role = await _roleManager.FindByIdAsync(request.RoleId.Value.ToString()).ConfigureAwait(false);
                    if (role is null)
                        return ServiceResponse<EntityStateInfo>.Error($"Role with ID {request.RoleId} not found.");
                    if (string.IsNullOrWhiteSpace(role.Name))
                        return ServiceResponse<EntityStateInfo>.Error($"Role with ID {request.RoleId} has no name.");

                    if (!await _userManager.IsInRoleAsync(user, role.Name).ConfigureAwait(false))
                    {
                        IdentityResult roleResult = await _userManager.AddToRoleAsync(user, role.Name).ConfigureAwait(false);
                        if (!roleResult.Succeeded)
                            return ServiceResponse<EntityStateInfo>.Error($"Error adding user to role.\n{roleResult.GetErrors()}");
                    }
                }

                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
                return ServiceResponse<EntityStateInfo>.Success(entityStateInfo);
            }
            catch (Exception ex)
            {
                string action = request.Id != Guid.Empty ? "updating" : "creating";
                return ServiceResponse<EntityStateInfo>.Error($"Error {action} user.\n{ex.GetFullMessage()}");
            }
        }

        public async Task<ServiceResponse<EntityStateInfo>> CreateUpdateUserAsync(CreateUpdateUserWithTenantRequest request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                UserEntity user = _mapper.Map<UserEntity>(request);

                EntityStateInfo entityStateInfo;
                if (request.Id != Guid.Empty)
                {
                    entityStateInfo = await UpdateUserAsync(user, cancellationToken).ConfigureAwait(false);
                    if (!entityStateInfo.Succeeded)
                        return ServiceResponse<EntityStateInfo>.Error(entityStateInfo.Message);
                }
                else
                {
                    entityStateInfo = await CreateUserAsync(user, request.Password, cancellationToken).ConfigureAwait(false);
                    if (!entityStateInfo.Succeeded)
                        return ServiceResponse<EntityStateInfo>.Error(entityStateInfo.Message);
                }

                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

                if (request.RoleId.HasValue)
                {
                    RoleEntity? role = await _roleManager.FindByIdAsync(request.RoleId.Value.ToString()).ConfigureAwait(false);
                    if (role is null)
                        return ServiceResponse<EntityStateInfo>.Error($"Role with ID {request.RoleId} not found.");
                    if (string.IsNullOrWhiteSpace(role.Name))
                        return ServiceResponse<EntityStateInfo>.Error($"Role with ID {request.RoleId} has no name.");

                    if (!await _userManager.IsInRoleAsync(user, role.Name).ConfigureAwait(false))
                    {
                        IdentityResult roleResult = await _userManager.AddToRoleAsync(user, role.Name).ConfigureAwait(false);
                        if (!roleResult.Succeeded)
                            return ServiceResponse<EntityStateInfo>.Error($"Error adding user to role.\n{roleResult.GetErrors()}");
                    }
                }

                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
                return ServiceResponse<EntityStateInfo>.Success(entityStateInfo);
            }
            catch (Exception ex)
            {
                string action = request.Id != Guid.Empty ? "updating" : "creating";
                return ServiceResponse<EntityStateInfo>.Error($"Error {action} user.\n{ex.GetFullMessage()}");
            }
        }

        private async Task<EntityStateInfo> CreateUserAsync(UserEntity user, string password, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                IdentityResult result = await _userManager.CreateAsync(user, password).ConfigureAwait(false);
                if (!result.Succeeded)
                    return new EntityStateInfo(message: $"Error creating user.\n{result.GetErrors()}");

                return new EntityStateInfo(null, $"User {user.UserName} created successfully", EntityStatus.Added);
            }
            catch (Exception ex)
            {
                return new EntityStateInfo(message: $"Error creating user.\n{ex.GetFullMessage()}");
            }
        }

        private async Task<EntityStateInfo> UpdateUserAsync(UserEntity user, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                IdentityResult result = await _userManager.UpdateAsync(user).ConfigureAwait(false);
                if (!result.Succeeded)
                    return new EntityStateInfo(message: $"Error updating user.\n{result.GetErrors()}");

                return new EntityStateInfo(user.Id.ToString(), $"User {user.UserName} updated successfully", EntityStatus.Updated);
            }
            catch (Exception ex)
            {
                return new EntityStateInfo(message: $"Error updating user.\n{ex.GetFullMessage()}");
            }
        }

        public async Task<ServiceResponse<EntityStateInfo>> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                UserEntity? user = await _userManager.FindByIdAsync(userId.ToString()).ConfigureAwait(false);
                if (user is null)
                    return ServiceResponse<EntityStateInfo>.Error($"User with ID {userId} not found.");

                user.IsDeleted = true;

                IdentityResult result = await _userManager.DeleteAsync(user).ConfigureAwait(false);
                if (!result.Succeeded)
                    return ServiceResponse<EntityStateInfo>.Error($"Error deleting user {user.UserName}.\n{result.GetErrors()}");

                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
                return ServiceResponse<EntityStateInfo>.Success(new EntityStateInfo(userId.ToString(), $"User {user.UserName} deleted successfully.", EntityStatus.Deleted));
            }
            catch (Exception ex)
            {
                return ServiceResponse<EntityStateInfo>.Error($"Error deleting user.\n{ex.GetFullMessage()}");
            }
        }

        public async Task<ServiceResponse> SignInAsync(string userName, string password, bool rememberMe, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                UserEntity? user = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);
                if (user == null)
                    return ServiceResponse.Error("User not found.");

                bool checkPwRes = await _userManager.CheckPasswordAsync(user, password).ConfigureAwait(false);
                if (!checkPwRes)
                    return ServiceResponse.Error("Invalid username or password.");

                bool isSystemAdmin = await _userManager.IsInRoleAsync(user, UserTenantClaimTypes.SysAdmin).ConfigureAwait(false);
                bool isTenantAdmin = await _userManager.IsInRoleAsync(user, UserTenantClaimTypes.TenantAdmin).ConfigureAwait(false);

                List<Claim> claims = new()
                {
                    new(UserTenantClaimTypes.TenantId, user.TenantId.ToString() ?? string.Empty),
                    new(UserTenantClaimTypes.SysAdmin, isSystemAdmin.ToString()),
                    new(UserTenantClaimTypes.TenantAdmin, isTenantAdmin.ToString())
                };

                List<Claim> roleClaims = [];
                if (isSystemAdmin)
                    roleClaims.Add(new(ClaimTypes.Role, UserTenantClaimTypes.SysAdmin));
                if (isTenantAdmin)
                    roleClaims.Add(new(ClaimTypes.Role, UserTenantClaimTypes.TenantAdmin));

                if (roleClaims.Count > 0)
                    claims.AddRange(roleClaims);

                IList<Claim> c = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
                if (c.Count > 0)
                    await _userManager.RemoveClaimsAsync(user, c).ConfigureAwait(false);

                IdentityResult claimResult = await _userManager.AddClaimsAsync(user, claims).ConfigureAwait(false);
                if (!claimResult.Succeeded)
                    return ServiceResponse.Error($"Error adding claims to user.\n{claimResult.GetErrors()}");

                await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

                if (_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true)
                    await _signInManager.SignOutAsync().ConfigureAwait(false);

                await _signInManager.SignInAsync(user, rememberMe).ConfigureAwait(false);

                return ServiceResponse.Success("Sign in successful.");
            }
            catch (Exception ex)
            {
                return ServiceResponse.Error($"Error signing in.\n{ex.GetFullMessage()}");
            }
        }

        public async Task<ServiceResponse> SignOutAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await _signInManager.SignOutAsync().ConfigureAwait(false);
                return ServiceResponse.Success("Sign out successful.");
            }
            catch (Exception ex)
            {
                return ServiceResponse.Error($"Error signing out.\n{ex.GetFullMessage()}");
            }
        }
    }
}
