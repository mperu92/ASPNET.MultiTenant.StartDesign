namespace StartTemplateNew.Shared.Models.Dto.Responses.ActionResult
{
    public class OkValueObject<T> : OkValueObject
    {
        public OkValueObject(T value, string? message = null)
            : base(message)
        {
            Data = value;
        }

        public T Data { get; set; }

        public static OkValueObject<T> Create(T value, string? message = null)
        {
            return new OkValueObject<T>(value, message);
        }
    }

    public class OkValueObject
    {
        public OkValueObject(string? message = null)
        {
            Message = message;
        }

        public string? Message { get; set; }

        public static OkValueObject Create(string? message = null)
        {
            return new OkValueObject(message);
        }
    }
}
