namespace StartTemplateNew.Shared.Models.Dto.Base.Requests
{
    public class BaseCommandRequest<TKey> : ICommandRequest<TKey>
    {
        public BaseCommandRequest() { }

        public BaseCommandRequest(TKey? contentId)
        {
            Id = contentId;
        }

        public TKey? Id { get; set; }
    }
}
