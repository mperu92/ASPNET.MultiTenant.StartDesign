using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StartTemplateNew.Shared.Helpers.Extensions;
using StartTemplateNew.Shared.Services.Core;
using StartTemplateNew.Shared.Services.Factories;

namespace StartTemplateNew.WebApi.Controllers.Base
{
    [ApiController]
    public class BaseApiController<TController> : ControllerBase
        where TController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;
        protected BaseApiController(ILogger<BaseApiController<TController>> logger, IServiceFactory serviceFactory, IOptionsMonitor<AppSettings>? options = null)
        {
            Logger = logger;
            Options = options;
            _serviceFactory = serviceFactory;
        }

        protected ILogger<BaseApiController<TController>> Logger { get; }
        protected IOptionsMonitor<AppSettings>? Options { get; }

        protected TService GetService<TService>()
            where TService : class, IService
        {
            ArgumentNullException.ThrowIfNull(_serviceFactory);

            return _serviceFactory.CreateService<TService>();
        }

        protected static Type ControllerType =>
            typeof(TController);

        protected static string GetSimpleErrorString(Exception ex, string? prefix = null)
        {
            return !string.IsNullOrWhiteSpace(prefix) ? prefix + '\n' : "Request threw an error.\n" +
                ex.GetFullMessage();
        }
    }
}
