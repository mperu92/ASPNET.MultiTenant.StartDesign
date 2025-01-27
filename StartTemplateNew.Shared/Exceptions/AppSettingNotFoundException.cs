using StartTemplateNew.Shared.Helpers.Const;
using System.Diagnostics;

namespace StartTemplateNew.Shared.Exceptions
{
    [DebuggerDisplay(ExceptionDefaults.AppSettingNotFound.DebuggerDisplay)]
    public class AppSettingNotFoundException : Exception
    {
        public AppSettingNotFoundException() { }

        public AppSettingNotFoundException(string? message)
            : base(message ?? ExceptionDefaults.AppSettingNotFound.DefaultMessage) { }

        public AppSettingNotFoundException(string? message, Exception? innerException)
            : base(message ?? ExceptionDefaults.AppSettingNotFound.DefaultMessage, innerException) { }
    }
}
