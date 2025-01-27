namespace StartTemplateNew.Shared.Models.Dto.Base
{
    public interface IKeyedDto<TKey> : IDto
        where TKey : notnull, new()
    {
        TKey Id { get; set; }
    }

    public interface IKeyedDto : IDto
    {
        string Id { get; set; }
    }
}
