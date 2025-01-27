using Microsoft.Extensions.DependencyInjection;
using StartTemplateNew.Shared.Services.Core;
using StartTemplateNew.Shared.Services.Factories;
using StartTemplateNew.Shared.Services.Factories.Impl;
using System.Reflection;

namespace StartTemplateNew.Shared.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            IEnumerable<Type> serviceInterfaces = assembly.GetTypes()
                .Where(t => t.IsInterface && typeof(IService).IsAssignableFrom(t) && t != typeof(IService));

            IEnumerable<Type> serviceImplementations = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IService).IsAssignableFrom(t));

            foreach (Type serviceInterface in serviceInterfaces)
            {
                Type? implementation = serviceImplementations.FirstOrDefault(t => t.GetInterfaces().Contains(serviceInterface));
                if (implementation != null)
                    services.Add(new ServiceDescriptor(serviceInterface, implementation, serviceLifetime));
            }

            services.Add(new(typeof(IServiceFactory), typeof(ServiceFactory), serviceLifetime));

            return services;
        }
    }
}
