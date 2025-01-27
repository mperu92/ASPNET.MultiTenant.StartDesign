namespace StartTemplateNew.DAL.Entities.Auditing
{
    public interface IExpiringInfoEntity : IAuditedEntity
    {
        DateTimeOffset? ExpirationDate { get; set; }
    }
}
