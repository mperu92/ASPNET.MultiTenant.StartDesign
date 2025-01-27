using StartTemplateNew.Shared.Helpers.Const;
using System.Diagnostics;

namespace StartTemplateNew.Shared.Exceptions
{
    [DebuggerDisplay(ExceptionDefaults.TokenGeneration.DebuggerDisplay)]
    public class TokenGenerationException : Exception
    {
        public TokenGenerationException() { }

        public TokenGenerationException(string? message)
            : base(message ?? ExceptionDefaults.TokenGeneration.DefaultMessage) { }

        public TokenGenerationException(string? message, Exception? innerException)
            : base(message ?? ExceptionDefaults.TokenGeneration.DefaultMessage, innerException) { }
    }
}
