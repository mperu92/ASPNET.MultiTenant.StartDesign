using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StartTemplateNew.Shared.Services.Factories;

namespace StartTemplateNew.WebApi.Controllers.Base.ApiVersions
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public abstract class V1ApiController<TController> : BaseApiController<TController>
        where TController : ControllerBase
    {
        protected V1ApiController(ILogger<V1ApiController<TController>> logger, IServiceFactory serviceFactory, IOptionsMonitor<AppSettings>? options = null)
            : base(logger, serviceFactory, options) { }
    }
}
