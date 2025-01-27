using Microsoft.Extensions.DependencyInjection;
using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Repositories.Core.Base;

namespace StartTemplateNew.DAL.Repositories.Factories.Impl
{
    public class RepositoryFactory(IServiceProvider serviceProvider) : IRepositoryFactory
    {
        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class, IKeyedEntity<TKey>
            where TKey : struct, IEquatable<TKey>
        {
            return (IRepository<TEntity, TKey>)serviceProvider.GetRequiredService(typeof(Repository<TEntity, TKey>));
        }

        public TRepository GetSpecificRepositoryInstance<TRepository>() where TRepository : class, IRepository
        {
            TRepository? repository = serviceProvider.GetService<TRepository>();
            return repository
                ?? throw new InvalidOperationException($"Repository of type {typeof(TRepository).Name} is not registered.");
        }
    }
}
