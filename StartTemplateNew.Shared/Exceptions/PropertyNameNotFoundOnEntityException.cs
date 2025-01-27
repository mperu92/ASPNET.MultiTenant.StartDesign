using StartTemplateNew.Shared.Helpers.Const;
using System.Diagnostics;

namespace StartTemplateNew.Shared.Exceptions
{
    [DebuggerDisplay(ExceptionDefaults.PropertyNameNotFoundOnEntity.DebuggerDisplay)]

    public class PropertyNameNotFoundOnEntityException : Exception
    {
        public PropertyNameNotFoundOnEntityException() { }

        public PropertyNameNotFoundOnEntityException(string? message)
            : base(message ?? ExceptionDefaults.PropertyNameNotFoundOnEntity.DefaultMessage) { }

        public PropertyNameNotFoundOnEntityException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.PropertyNameNotFoundOnEntity.DefaultMessage, innerException) { }
    }
}
