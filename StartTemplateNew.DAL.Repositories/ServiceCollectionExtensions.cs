using Microsoft.Extensions.DependencyInjection;
using StartTemplateNew.DAL.Repositories.Core.Base;
using StartTemplateNew.DAL.Repositories.Factories;
using StartTemplateNew.DAL.Repositories.Factories.Impl;
using System.Reflection;

namespace StartTemplateNew.DAL.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            if (serviceLifetime != ServiceLifetime.Scoped)
                throw new NotSupportedException("Only scoped service lifetime is supported.");

            // Registra il tipo generico aperto
            services.Add(new ServiceDescriptor(typeof(IRepository<,>), typeof(Repository<,>), serviceLifetime));
            // Registra il tipo concreto
            services.Add(new ServiceDescriptor(typeof(Repository<,>), typeof(Repository<,>), serviceLifetime));

            // Registra le implementazioni specifiche
            Assembly assembly = Assembly.GetExecutingAssembly();
            IEnumerable<Type> repoInterfaces = assembly.GetTypes()
                .Where(t => t.IsInterface && Array.Exists(t.GetInterfaces(), i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<,>)));

            IEnumerable<Type> repoImplementations = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.BaseType?.IsGenericType == true && t.BaseType.GetGenericTypeDefinition() == typeof(Repository<,>));

            foreach (Type repoInterface in repoInterfaces)
            {
                Type? implementation = repoImplementations.FirstOrDefault(t => t.GetInterfaces().Contains(repoInterface));
                if (implementation != null)
                    services.Add(new ServiceDescriptor(repoInterface, implementation, serviceLifetime));
            }

            services.Add(new(typeof(IRepositoryFactory), typeof(RepositoryFactory), serviceLifetime));

            services.Add(new ServiceDescriptor(typeof(IClaimedRepository<,,,>), typeof(ClaimedRepository<,,,>), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ClaimedRepository<,,,>), typeof(ClaimedRepository<,,,>), serviceLifetime));

            IEnumerable<Type> claimedRepoInterfaces = assembly.GetTypes()
                .Where(t => t.IsInterface && Array.Exists(t.GetInterfaces(), i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IClaimedRepository<,,,>)));

            IEnumerable<Type> claimedRepoImplementations = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.BaseType?.IsGenericType == true && t.BaseType.GetGenericTypeDefinition() == typeof(ClaimedRepository<,,,>));

            foreach (Type repoInterface in claimedRepoInterfaces)
            {
                Type? implementation = claimedRepoImplementations.FirstOrDefault(t => t.GetInterfaces().Contains(repoInterface));
                if (implementation != null)
                    services.Add(new(repoInterface, implementation, serviceLifetime));
            }

            services.Add(new(typeof(IClaimedRepositoryFactory), typeof(ClaimedRepositoryFactory), serviceLifetime));

            services.Add(new ServiceDescriptor(typeof(ITenantedRepository<,,,,,>), typeof(TenantedRepository<,,,,,>), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(TenantedRepository<,,,,,>), typeof(TenantedRepository<,,,,,>), serviceLifetime));

            IEnumerable<Type> tenantRepoInterfaces = assembly.GetTypes()
                .Where(t => t.IsInterface && Array.Exists(t.GetInterfaces(), i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITenantedRepository<,,,,,>)));

            IEnumerable<Type> tenantRepoImplementations = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.BaseType?.IsGenericType == true && t.BaseType.GetGenericTypeDefinition() == typeof(TenantedRepository<,,,,,>));

            foreach (Type repoInterface in tenantRepoInterfaces)
            {
                Type? implementation = tenantRepoImplementations.FirstOrDefault(t => t.GetInterfaces().Contains(repoInterface));
                if (implementation != null)
                    services.Add(new(repoInterface, implementation, serviceLifetime));
            }

            services.Add(new(typeof(ITenantedRepositoryFactory), typeof(TenantedRepositoryFactory), serviceLifetime));

            return services;
        }
    }
}
