using System.Diagnostics;

namespace StartTemplateNew.DAL.Repositories.Exceptions
{
    [DebuggerDisplay("ReturnBehaviorNotFoundException: {Message}")]
    public class ReturnBehaviorNotFoundException : Exception
    {
        private const string _defaultMessage = "Return behavior not found";

        public ReturnBehaviorNotFoundException()
            : base(_defaultMessage) { }

        public ReturnBehaviorNotFoundException(string? message)
            : base(message ?? _defaultMessage) { }

        public ReturnBehaviorNotFoundException(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}
