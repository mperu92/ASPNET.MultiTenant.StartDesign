using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StartTemplateNew.DAL.Contexts;

namespace StartTemplateNew.DAL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, string connectionString, bool useLazyLoadingProxies = true, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies(useLazyLoadingProxies);
                options.UseSqlServer(connectionString);
                options.EnableSensitiveDataLogging();
            }, serviceLifetime);

            return services;
        }
    }
}
