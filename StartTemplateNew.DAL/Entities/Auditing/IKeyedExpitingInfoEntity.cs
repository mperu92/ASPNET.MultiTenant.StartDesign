using StartTemplateNew.DAL.Entities.Base;

namespace StartTemplateNew.DAL.Entities.Auditing
{
    public interface IKeyedExpiringInfoEntity<TEntity> : IExpiringInfoEntity
        where TEntity : class, IEntity
    {
        TEntity? ExpirationSetBy { get; set; }
    }
}
