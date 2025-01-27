using StartTemplateNew.DAL.Entities.Base;

namespace StartTemplateNew.DAL.Entities.Auditing
{
    public interface IKeyedCreateUpdateInfoEntity<TEntity> : ICreateUpdateInfoEntity
        where TEntity : class, IEntity
    {
        TEntity CreatedBy { get; set; }
        TEntity? UpdatedBy { get; set; }
    }
}
