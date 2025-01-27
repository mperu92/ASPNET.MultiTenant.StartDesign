using StartTemplateNew.DAL.Entities.Base;

namespace StartTemplateNew.DAL.Entities.Auditing
{
    public interface IKeyedDeleteInfoEntity<TUser> : IDeleteInfoEntity
        where TUser : class, IEntity
    {
        TUser? DeletedBy { get; set; }
    }
}
