using StartTemplateNew.DAL.Entities.Base;
using StartTemplateNew.DAL.Repositories.Core.Base;

namespace StartTemplateNew.DAL.Repositories.Factories
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class, IKeyedEntity<TKey>
            where TKey : struct, IEquatable<TKey>;

        TRepository GetSpecificRepositoryInstance<TRepository>() where TRepository : class, IRepository;
    }
}
