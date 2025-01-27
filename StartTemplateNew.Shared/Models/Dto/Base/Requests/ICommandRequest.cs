namespace StartTemplateNew.Shared.Models.Dto.Base.Requests
{
    public interface ICommandRequest<TKey>
    {
        TKey? Id { get; set; }
    }
}
