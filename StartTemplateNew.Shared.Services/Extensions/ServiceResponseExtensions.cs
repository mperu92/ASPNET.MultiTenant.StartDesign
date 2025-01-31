using StartTemplateNew.Shared.Services.Models;

namespace StartTemplateNew.Shared.Services.Extensions
{
    public static class ServiceResponseExtensions
    {
        public static async Task<ServiceResponse<TNext>> Continue<T, TNext>(this Task<ServiceResponse<T>> task, Func<Task<ServiceResponse<TNext>>> next)
        {
            ServiceResponse<T> result = await task.ConfigureAwait(false);
            return result.IsSuccess ? await next().ConfigureAwait(false) : ServiceResponse<TNext>.Error(result.Message);
        }

        public static async Task<ServiceResponse<TNext>> Continue<T, TNext>(this Task<ServiceResponse<T>> task, Func<T, Task<ServiceResponse<TNext>>> next)
        {
            ServiceResponse<T> result = await task.ConfigureAwait(false);
            return result.IsSuccessWithData ? await next(result.Data).ConfigureAwait(false) : ServiceResponse<TNext>.Error(result.Message);
        }

        public static async Task<ServiceResponse<T>> Meanwhile<T>(this Task<ServiceResponse<T>> task, Action<T> action)
        {
            ServiceResponse<T> result = await task.ConfigureAwait(false);
            if (result.IsSuccessWithData)
                action(result.Data);

            return result;
        }

        public static async Task<ServiceResponse<T>> Meanwhile<T>(this Task<ServiceResponse<T>> task, Func<T, Task> func)
        {
            ServiceResponse<T> result = await task.ConfigureAwait(false);
            if (result.IsSuccessWithData)
                await func(result.Data).ConfigureAwait(false);

            return result;
        }

    }
}
