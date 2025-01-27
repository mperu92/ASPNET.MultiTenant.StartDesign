namespace StartTemplateNew.DAL.Entities.Auditing
{
    public interface IActivationExpiringInfoEntity : IExpiringInfoEntity
    {
        DateTimeOffset? ActivationDate { get; set; }
    }
}
