using Microsoft.Extensions.DependencyInjection;
using StartTemplateNew.DAL.TenantUserProvider.Core;
using StartTemplateNew.DAL.TenantUserProvider.Core.Impl;
using StartTemplateNew.DAL.TenantUserProvider.Helpers;
using System.Reflection;

namespace StartTemplateNew.DAL.TenantUserProvider
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClaimUserProvider(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.Add(new ServiceDescriptor(typeof(ITheTypeConverter<Guid>), typeof(TheTypeConverter<Guid>), ServiceLifetime.Singleton));
            services.Add(new ServiceDescriptor(typeof(ITheTypeConverter<int>), typeof(TheTypeConverter<int>), ServiceLifetime.Singleton));
            services.Add(new ServiceDescriptor(typeof(ITheTypeConverter<string>), typeof(TheTypeConverter<string>), ServiceLifetime.Singleton));

            services.Add(new ServiceDescriptor(typeof(ITheTypeConverter<>), typeof(TheTypeConverter<>), ServiceLifetime.Singleton));

            if (serviceLifetime != ServiceLifetime.Scoped)
                throw new NotSupportedException("Only Scoped service lifetime is supported for TenantProvider.");

            services.Add(new ServiceDescriptor(typeof(IPrincipalProvider), typeof(PrincipalProvider), serviceLifetime));
            // Registra il tipo concreto
            //services.Add(new ServiceDescriptor(typeof(PrincipalProvider<>), typeof(PrincipalProvider<>), serviceLifetime))

            Assembly assembly = Assembly.GetExecutingAssembly();

            IEnumerable<Type> providerInterfaces = assembly.GetTypes()
             .Where(t => t.IsInterface && Array.Exists(t.GetInterfaces(), i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPrincipalProvider)));

            IEnumerable<Type> providerImplementations = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.BaseType?.IsGenericType == true && t.BaseType.GetGenericTypeDefinition() == typeof(PrincipalProvider));

            foreach (Type providerInterface in providerInterfaces)
            {
                Type? implementation = providerImplementations.FirstOrDefault(t => t.GetInterfaces().Contains(providerInterface));
                if (implementation != null)
                    services.Add(new ServiceDescriptor(providerInterface, implementation, serviceLifetime));
            }

            return services;
        }
    }
}
