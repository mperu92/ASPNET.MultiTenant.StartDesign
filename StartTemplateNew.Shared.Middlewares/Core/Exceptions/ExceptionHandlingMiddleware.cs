using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using StartTemplateNew.Shared.Middlewares.Models;

namespace StartTemplateNew.Shared.Middlewares.Core.Exceptions
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex).ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            string message = "An error occurred";
            int statusCode = StatusCodes.Status500InternalServerError;

            if (exception is OperationCanceledException)
            {
                statusCode = StatusCodes.Status408RequestTimeout;
                message = "Request has been canceled.";
            }

            if (exception is UnauthorizedAccessException)
            {
                statusCode = StatusCodes.Status401Unauthorized;
                message = "Unauthorized access.";
            }

            context.Response.StatusCode = statusCode;

            ErrorResponse errorResponse = ErrorResponse.CreateFromException(message, exception);
            return context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
    }
}
