using StartTemplateNew.Shared.Services.Models;

namespace StartTemplateNew.Shared.Services.Extensions
{
    public static class ServiceResponseExtensions
    {
        /// <summary>
        /// Awaits the task and calls the next function if the task is successful.
        /// </summary>
        public static async Task<ServiceResponse<TNext>> Continue<T, TNext>(this Task<ServiceResponse<T>> task, Func<Task<ServiceResponse<TNext>>> next)
        {
            ServiceResponse<T> result = await task.ConfigureAwait(false);
            return result.IsSuccess ? await next().ConfigureAwait(false) : ServiceResponse<TNext>.Error(result.Message);
        }

        /// <summary>
        /// Awaits the task and calls the next function if the task is successful.
        /// </summary>
        public static async Task<ServiceResponse<TNext>> Continue<T, TNext>(this Task<ServiceResponse<T>> task, Func<T, Task<ServiceResponse<TNext>>> next)
        {
            ServiceResponse<T> result = await task.ConfigureAwait(false);
            return result.IsSuccessWithData ? await next(result.Data).ConfigureAwait(false) : ServiceResponse<TNext>.Error(result.Message);
        }

        /// <summary>
        /// Executes an operation without modifying the response.
        /// </summary>
        public static async Task<ServiceResponse<T>> Meanwhile<T>(this Task<ServiceResponse<T>> task, Action<T> action)
        {
            ServiceResponse<T> result = await task.ConfigureAwait(false);
            if (result.IsSuccessWithData)
                action(result.Data);

            return result;
        }

        /// <summary>
        /// Executes an operation without modifying the response.
        /// </summary>
        public static async Task<ServiceResponse<T>> Meanwhile<T>(this Task<ServiceResponse<T>> task, Func<T, Task> func)
        {
            ServiceResponse<T> result = await task.ConfigureAwait(false);
            if (result.IsSuccessWithData)
                await func(result.Data).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Awaits the task and calls the next function if the task is successful.
        /// WARNING: This method brokes the chain, it is intended to be used at the end of the chain.
        /// </summary>
        public static async Task<ServiceResponse> EndsWithNoValue<T>(this Task<ServiceResponse<T>> task, Func<T, Task<ServiceResponse>> next)
        {
            ServiceResponse<T> result = await task.ConfigureAwait(false);
            return result.IsSuccessWithData ? await next(result.Data).ConfigureAwait(false) : ServiceResponse.Error(result.Message);
        }
    }
}
