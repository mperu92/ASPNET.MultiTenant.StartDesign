namespace StartTemplateNew.Shared.Models.Dto.Responses.ActionResult
{
    public readonly struct BadRequestObjectValidationValue(string title, IDictionary<string, string[]> validationErrors)
    {
        public string Title { get; } = title;
        public IDictionary<string, string[]> ValidationErrors { get; } = validationErrors;
    }
}
