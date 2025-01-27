using Microsoft.Extensions.DependencyInjection;
using StartTemplateNew.Shared.Services.Core;

namespace StartTemplateNew.Shared.Services.Factories.Impl
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TService CreateService<TService>()
            where TService : class, IService
        {
            return (TService)_serviceProvider.GetRequiredService(typeof(TService));
        }
    }
}
