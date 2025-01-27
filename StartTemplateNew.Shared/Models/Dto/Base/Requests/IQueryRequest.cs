namespace StartTemplateNew.Shared.Models.Dto.Base.Requests
{
    public interface IQueryRequest
    {
        string? SearchText { get; set; }

        string? OrderField { get; set; }
        bool IsAscending { get; set; }
    }
}
