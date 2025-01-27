using Microsoft.Extensions.DependencyInjection;
using StartTemplateNew.DAL.UnitOfWork.Core;
using _UnitOfWork = StartTemplateNew.DAL.UnitOfWork.Core.Impl.UnitOfWork;

namespace StartTemplateNew.DAL.UnitOfWork
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.Add(new(typeof(IUnitOfWork), typeof(_UnitOfWork), serviceLifetime));
            return services;
        }
    }
}
