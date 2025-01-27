namespace StartTemplateNew.Shared.Models.Dto.Validation
{
    public readonly struct DtoValidationResult(bool isValid, string? message = null)
    {
        public bool IsValid { get; } = isValid;
        public string? Message { get; } = message;

        public static DtoValidationResult Success(string? message = null) => new(true, message);
        public static DtoValidationResult Fail(string? message = null) => new(false, message);

        public static implicit operator bool(DtoValidationResult result) => result.IsValid;
    }
}
