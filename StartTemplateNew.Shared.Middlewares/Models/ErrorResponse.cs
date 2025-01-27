using Newtonsoft.Json;
using StartTemplateNew.Shared.ExceptionHelpers.Extensions;

namespace StartTemplateNew.Shared.Middlewares.Models
{
    public readonly struct ErrorResponse
    {
        public ErrorResponse(string errorMessage, string? errorCode = null, Exception? exception = null, string? stackTrace = null)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Exception = exception;
            StackTrace = stackTrace;
        }

        [JsonProperty("error")]
        public string ErrorMessage { get; }

        public string? ErrorCode { get; }

        public Exception? Exception { get; }

        public string? StackTrace { get; }

        public static ErrorResponse Create(string errorMessage, string? errorCode = null, Exception? exception = null)
        {
            return new ErrorResponse(errorMessage, errorCode, exception, exception?.StackTrace);
        }

        public static ErrorResponse CreateFromException(string errorMessage, Exception exception, string? errorCode = null)
        {
            string msg = $"{errorMessage}.\n{exception.GetAllMessages()}";
            return new ErrorResponse(msg, errorCode, exception, stackTrace: exception.StackTrace);
        }
    }
}
