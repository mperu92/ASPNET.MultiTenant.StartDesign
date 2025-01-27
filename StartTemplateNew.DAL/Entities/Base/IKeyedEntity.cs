namespace StartTemplateNew.DAL.Entities.Base
{
    public interface IKeyedEntity<TKey> : IEntity
        where TKey : struct, IEquatable<TKey>
    {
        TKey Id { get; set; }
    }
}
