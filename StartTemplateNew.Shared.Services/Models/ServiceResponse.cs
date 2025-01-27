using StartTemplateNew.Shared.Models.Paging;
using StartTemplateNew.Shared.Services.Enums;
using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.Shared.Services.Models
{
    public class ServiceResponse<T> : ServiceResponse
    {
        public ServiceResponse() { }

        public ServiceResponse(T? data, bool isSuccess, string? message = null)
            : base(isSuccess, message) => Data = data;

        /// <summary>
        /// Constructor intended to represents a success response with data and pagination, that are mandatory.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pagination"></param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException"></exception>"
        public ServiceResponse(T data, Pagination pagination, string? message = null)
            : base(ServiceResponseStatus.Success, message)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(pagination);

            Data = data;
            Pagination = pagination;
        }

        public ServiceResponse(T? data, ServiceResponseStatus status, string? message = null)
            : base(status, message) => Data = data;

        public readonly T? Data;

        public bool HasData
        {
            [MemberNotNullWhen(true, nameof(Data))]
            get => Data is not null;
        }

        public Pagination? Pagination { get; set; }

        public bool HasPagination
        {
            [MemberNotNullWhen(true, nameof(Pagination))]
            get => Pagination is not null;
        }

        public static ServiceResponse<T> Success(T? data, string? message = null) => new(data, true, message);

        /// <summary>
        /// Success response with data and pagination, that are mandatory.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pagination"></param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException"></exception>"
        public static ServiceResponse<T> Success(T data, Pagination pagination, string? message = null)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(pagination);

            return new(data, pagination, message);
        }

        new public static ServiceResponse<T> Error(string? message = null) => new(default, false, message);
        public static ServiceResponse<T> Warning(T? data, string? message = null) => new(data, ServiceResponseStatus.Warning, message);

        public static implicit operator bool(ServiceResponse<T> response) => response.IsSuccess;

        public static implicit operator T?(ServiceResponse<T> response) => response.Data;
    }

    public class ServiceResponse
    {
        public ServiceResponse() { }

        public ServiceResponse(bool isSuccess, string? message = null)
         : this(isSuccess ? ServiceResponseStatus.Success : ServiceResponseStatus.Error, message) { }

        public ServiceResponse(ServiceResponseStatus status, string? message = null)
        {
            Status = status;
            Message = message;
        }

        public ServiceResponseStatus Status { get; set; } = ServiceResponseStatus.Success;

        public bool IsSuccess => Status == ServiceResponseStatus.Success;
        public bool IsError => Status == ServiceResponseStatus.Error;
        public bool IsWarning => Status == ServiceResponseStatus.Warning;

        public string? Message { get; set; }
        public bool HasMessage
        {
            [MemberNotNullWhen(true, nameof(Message))]
            get => !string.IsNullOrEmpty(Message);
        }

        public static ServiceResponse Success(string? message = null) => new(true, message);
        public static ServiceResponse Error(string? message = null) => new(false, message);
        public static ServiceResponse Warning(string? message = null) => new(ServiceResponseStatus.Warning, message);

        public static implicit operator bool(ServiceResponse response) => response.IsSuccess;
    }
}
