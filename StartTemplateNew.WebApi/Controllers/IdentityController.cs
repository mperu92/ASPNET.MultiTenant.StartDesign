using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StartTemplateNew.Shared.Helpers.Attributes;
using StartTemplateNew.Shared.Models.Dto;
using StartTemplateNew.Shared.Models.Dto.Base.Responses;
using StartTemplateNew.Shared.Models.Dto.Identity;
using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Services.Core;
using StartTemplateNew.Shared.Services.Factories;
using StartTemplateNew.Shared.Services.Models;
using StartTemplateNew.WebApi.Controllers.Base.ApiVersions;

namespace StartTemplateNew.WebApi.Controllers
{
    [ControllerNestedRoute(".id", typeof(IdentityController))]
    public class IdentityController : V1ApiController<IdentityController>
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public IdentityController(ILogger<IdentityController> logger, IServiceFactory serviceFactory, IOptionsMonitor<AppSettings> options)
            : base(logger, serviceFactory, options)
        {
            _userService = GetService<IUserService>();
            _roleService = GetService<IRoleService>();
        }

        [Authorize(Roles = "SysAdmin, TenantAdmin")]
        [HttpPost("create-user")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody] CreateUpdateUserRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            try
            {
                ServiceResponse<EntityStateInfo> response = await _userService.CreateUpdateUserAsync(request, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);

                return Ok(response.Data.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating user.");
                return BadRequest(GetSimpleErrorString(ex));
            }
        }

        [Authorize(Roles = "SysAdmin")]
        [HttpPost("create-user-admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> CreateSysAdmin([FromBody] CreateUpdateUserWithTenantRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            try
            {
                ServiceResponse<EntityStateInfo> response = await _userService.CreateUpdateUserAsync(request, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);

                return Ok(response.Data.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating user.");
                return BadRequest(GetSimpleErrorString(ex));
            }
        }

        [Authorize(Roles = "SysAdmin, TenantAdmin")]
        [HttpPost("create-user-form")]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces("application/json")]
        public async Task<IActionResult> CreateForm([FromForm] CreateUpdateUserFormRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            try
            {
                ServiceResponse response = await _userService.CreateUserFormAsync(request, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);

                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating user form.");
                return BadRequest(GetSimpleErrorString(ex));
            }
        }

        [Authorize(Roles = "SysAdmin")]
        [HttpPost("create-role")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            try
            {
                ServiceResponse response = await _roleService.CreateRoleAsync(request, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);

                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating role.");
                return BadRequest(GetSimpleErrorString(ex));
            }
        }

        [HttpPost("login")]
        [Produces("application/json")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest loginRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                ServiceResponse response = await _userService.SignInAsync(loginRequest.Username, loginRequest.Password, loginRequest.RememberMe, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);

                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting roles.");
                return BadRequest(GetSimpleErrorString(ex));
            }
        }

        [HttpGet("logout")]
        [Produces("application/json")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken = default)
        {
            try
            {
                ServiceResponse response = await _userService.SignOutAsync(cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);

                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting roles.");
                return BadRequest(GetSimpleErrorString(ex));
            }
        }

        [Authorize(Roles = "SysAdmin")]
        [HttpGet("roles")]
        [Produces("application/json")]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken = default)
        {
            try
            {
                ServiceResponse<ICollection<Role>> response = await _roleService.GetRolesAsync(cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting roles.");
                return BadRequest(GetSimpleErrorString(ex));
            }
        }

        [Authorize(Roles = "SysAdmin, TenantAdmin")]
        [HttpPost("users")]
        [Produces("application/json")]
        public async Task<IActionResult> GetUsers([FromBody] GetUsersRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                ServiceResponse<ICollection<User>> response = await _userService.GetUsersAsync(request, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                {
                    return BadRequest(response.Message);
                }
                else if (!response.HasData)
                {
                    string msg = $"Data is null. Expected at least an empty collection.\n{response.Message}";
                    Logger.LogError("{Msg}", msg);
                    return BadRequest(msg);
                }
                else if (!response.HasPagination)
                {
                    string msg = $"Pagination is null. Expected at least a default pagination.\n{response.Message}";
                    Logger.LogError("{Msg}", msg);
                    return BadRequest(msg);
                }

                return Ok(new OkPaginatedObject<User>(response.Data, response.Pagination, response.Message));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting users.");
                return BadRequest(GetSimpleErrorString(ex));
            }
        }
    }
}
