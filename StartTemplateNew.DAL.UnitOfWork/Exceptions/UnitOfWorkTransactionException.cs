using System.Diagnostics;

namespace StartTemplateNew.DAL.UnitOfWork.Exceptions
{
    [DebuggerDisplay("UnitOfWorkTransactionException: {Message}")]
    public class UnitOfWorkTransactionException : Exception
    {
        public UnitOfWorkTransactionException(string? message)
            : base(message) { }

        public UnitOfWorkTransactionException(string? message, Exception? innerException)
            : base(message, innerException) { }

        public UnitOfWorkTransactionException() { }
    }
}
