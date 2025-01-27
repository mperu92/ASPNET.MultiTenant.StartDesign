namespace StartTemplateNew.DAL.Entities.Auditing
{
    public interface IDeleteInfoEntity : IAuditedEntity
    {
        DateTimeOffset? DeletedAt { get; set; }
        bool IsDeleted { get; set; }
    }
}
