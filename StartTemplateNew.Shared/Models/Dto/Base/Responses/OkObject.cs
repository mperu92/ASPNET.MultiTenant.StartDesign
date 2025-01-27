namespace StartTemplateNew.Shared.Models.Dto.Base.Responses
{
    public class OkObject<T>
            where T : new()
    {
        private const string _okMessage = "OK";

        public OkObject(T data)
        {
            Value = data;
        }

        public OkObject(T data, string? message)
        {
            Value = data;
            if (!string.IsNullOrWhiteSpace(message) && message != _okMessage)
                Message = message;
        }

        public T Value { get; }
        public string Message { get; } = _okMessage;

        public static OkObject<T> Default => new(new T());

        public static implicit operator OkObject<T>(T data) => new(data);
    }

    public class OkObject
    {
        private const string _okMessage = "OK";

        public OkObject() { }

        public OkObject(string? message)
        {
            if (!string.IsNullOrWhiteSpace(message) && message != _okMessage)
                Message = message;
        }

        public string Message { get; } = _okMessage;
    }
}
