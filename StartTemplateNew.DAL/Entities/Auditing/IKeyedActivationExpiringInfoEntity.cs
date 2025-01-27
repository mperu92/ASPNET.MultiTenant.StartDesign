using StartTemplateNew.DAL.Entities.Base;

namespace StartTemplateNew.DAL.Entities.Auditing
{
    public interface IKeyedActivationExpiringInfoEntity<TEntity> : IKeyedExpiringInfoEntity<TEntity>, IActivationExpiringInfoEntity
        where TEntity : class, IEntity
    {
        public TEntity? ActivationSetBy { get; set; }
    }
}
