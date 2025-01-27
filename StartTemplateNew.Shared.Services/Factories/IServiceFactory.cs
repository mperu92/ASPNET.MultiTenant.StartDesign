using StartTemplateNew.Shared.Services.Core;

namespace StartTemplateNew.Shared.Services.Factories
{
    public interface IServiceFactory
    {
        public TService CreateService<TService>()
            where TService : class, IService;
    }
}
