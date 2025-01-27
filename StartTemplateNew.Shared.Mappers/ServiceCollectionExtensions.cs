using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace StartTemplateNew.Shared.Mappers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppAutoMapper(this IServiceCollection services)
        {
            Assembly[] ass = AppDomain.CurrentDomain.GetAssemblies();

            services.AddAutoMapper(ass);

            return services;
        }
    }
}
