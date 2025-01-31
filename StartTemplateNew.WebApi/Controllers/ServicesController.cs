using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StartTemplateNew.Shared.Helpers.Attributes;
using StartTemplateNew.Shared.Helpers.Extensions;
using StartTemplateNew.Shared.Models.Dto;
using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Models.Dto.Services;
using StartTemplateNew.Shared.Services.Domain;
using StartTemplateNew.Shared.Services.Factories;
using StartTemplateNew.Shared.Services.Models;
using StartTemplateNew.WebApi.Controllers.Base.ApiVersions;

namespace StartTemplateNew.WebApi.Controllers
{
    [ControllerNestedRoute(".services", typeof(ServicesController))]
    public class ServicesController : V1ApiController<ServicesController>
    {
        private readonly IServiceService _servicesService;

        public ServicesController(ILogger<ServicesController> logger, IServiceFactory serviceFactory, IOptionsMonitor<AppSettings> options)
            : base(logger, serviceFactory, options)
        {
            _servicesService = GetService<IServiceService>();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetServices(CancellationToken cancellationToken = default)
        {
            try
            {
                ServiceResponse<ICollection<Service>> response = await _servicesService.GetServicesAsync(cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);
                if (!response.HasData)
                    return BadRequest(response.Message);

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetFullMessage());
            }
        }

        [Authorize(Roles = "SysAdmin,TenantAdmin")]
        [HttpPost("create-update")]
        public async Task<IActionResult> CreateUpdateService([FromBody] CreateUpdateServiceRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                ServiceResponse<EntityStateInfo> response = await _servicesService.CreateUpdateServiceAsync(request, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetFullMessage());
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteService(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                ServiceResponse<EntityStateInfo> response = await _servicesService.DeleteServiceAsync(id, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetFullMessage());
            }
        }

        [Authorize(Roles = "SysAdmin")]
        [HttpPost("set-tenantservice")]
        public async Task<IActionResult> SetTenantService([FromBody] SetTenantServiceRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                ServiceResponse<EntityStateInfo> response = await _servicesService.SetTenantServiceAsync(request, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccess)
                    return BadRequest(response.Message);

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetFullMessage());
            }
        }
    }
}
