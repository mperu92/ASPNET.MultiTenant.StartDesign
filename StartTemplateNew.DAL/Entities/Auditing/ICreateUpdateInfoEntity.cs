namespace StartTemplateNew.DAL.Entities.Auditing
{
    public interface ICreateUpdateInfoEntity : IAuditedEntity
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset? UpdatedAt { get; set; }
    }
}
