using Microsoft.Data.SqlClient;
using StartTemplateNew.Shared.ExceptionHelpers.Const;
using StartTemplateNew.Shared.ExceptionHelpers.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace StartTemplateNew.Shared.ExceptionHelpers.Core
{
    public static class SqlExceptionHelper
    {
        [DoesNotReturn]
        public static void HandleSqlException(this SqlException sqlException)
        {
            StringBuilder sb = new();
            sb.AppendLine("An error occurred while saving changes to the database.");
            sb.AppendLine(GetExceptionMessageBySqlErrorNummber(sqlException));

            throw new InvalidOperationException(sb.ToString(), sqlException);
        }

        [DoesNotReturn]
        public static void HandleSqlException(this SqlException sqlException, Type customExceptionType)
        {
            if (!typeof(Exception).IsAssignableFrom(customExceptionType))
                throw new ArgumentException("The custom exception type must be a subclass of Exception.");

            StringBuilder sb = new();
            sb.AppendLine("An error occurred while saving changes to the database.");
            sb.AppendLine(GetExceptionMessageBySqlErrorNummber(sqlException));

            string exceptionMsg = sb.ToString();

            Throw(customExceptionType, exceptionMsg, sqlException);
        }

        [DoesNotReturn]
        public static void HandleSqlException<TException>(this SqlException sqlException)
            where TException : Exception
        {
            Type customExceptionType = typeof(TException);
            if (!typeof(Exception).IsAssignableFrom(customExceptionType))
                throw new ArgumentException("The custom exception type must be a subclass of Exception.");

            StringBuilder sb = new();
            sb.AppendLine("An error occurred while saving changes to the database.");
            sb.AppendLine(GetExceptionMessageBySqlErrorNummber(sqlException));

            string exceptionMsg = sb.ToString();

            Throw(customExceptionType, exceptionMsg, sqlException);
        }

        [DoesNotReturn]
        private static void Throw(Type customExceptionType, string exceptionMsg, SqlException sqlException)
        {
            object? customException = Activator.CreateInstance(customExceptionType, exceptionMsg, sqlException);
            throw customException as Exception
                ?? new InvalidOperationException(exceptionMsg, sqlException);
        }

        private static string GetExceptionMessageBySqlErrorNummber(SqlException sqlException)
        {
            return sqlException.Number switch
            {
                SqlServerErrorNumbers.ForeignKeyViolation => $"There was a problem with one of the foreign keys in the database.{sqlException.GetAllMessages()}",
                SqlServerErrorNumbers.UniqueConstraintViolation or SqlServerErrorNumbers.UniqueIndexViolation => $"A unique constraint was violated.{sqlException.GetAllMessages()}",
                SqlServerErrorNumbers.Deadlock => $"There was a deadlock in the database.{sqlException.GetAllMessages()}",// Handle deadlock error
                SqlServerErrorNumbers.TimeoutExpired => $"Timeout expired.{sqlException.GetAllMessages()}",// Handle timeout error
                SqlServerErrorNumbers.ArithmeticOverflow => $"An arithmetic overflow occurred.{sqlException.GetAllMessages()}",// Handle arithmetic overflow error
                SqlServerErrorNumbers.StringOrBinaryDataWouldBeTruncated => $"String or binary data would be truncated.{sqlException.GetAllMessages()}",// Handle string or binary data would be truncated error
                SqlServerErrorNumbers.DivideByZero => $"Divide by zero error.{sqlException.GetAllMessages()}",// Handle divide by zero error
                SqlServerErrorNumbers.ErrorInAssignment => $"Error in assignment.{sqlException.GetAllMessages()}",// Handle error in assignment error
                SqlServerErrorNumbers.ErrorInConversion => $"Error in conversion.{sqlException.GetAllMessages()}",// Handle error in conversion error
                SqlServerErrorNumbers.ErrorInComparison => $"Error in comparison.{sqlException.GetAllMessages()}",// Handle error in comparison error
                SqlServerErrorNumbers.ErrorInGrouping => $"Error in grouping.{sqlException.GetAllMessages()}",// Handle error in grouping error
                SqlServerErrorNumbers.ErrorInSubquery => $"Error in subquery.{sqlException.GetAllMessages()}",// Handle error in subquery error
                SqlServerErrorNumbers.ErrorInAggregate => $"Error in aggregate.{sqlException.GetAllMessages()}",// Handle error in aggregate error
                SqlServerErrorNumbers.ErrorInJoin => $"Error in join.{sqlException.GetAllMessages()}",// Handle error in join error
                SqlServerErrorNumbers.ErrorInExists => $"Error in exists.{sqlException.GetAllMessages()}",// Handle error in exists error
                SqlServerErrorNumbers.ErrorInLike => $"Error in like.{sqlException.GetAllMessages()}",// Handle error in like error
                SqlServerErrorNumbers.ErrorInExpression => $"Error in expression.{sqlException.GetAllMessages()}",// Handle error in expression error
                SqlServerErrorNumbers.ErrorInFunction => $"Error in function.{sqlException.GetAllMessages()}",// Handle error in function error
                SqlServerErrorNumbers.ErrorInProcedure => $"Error in procedure.{sqlException.GetAllMessages()}",// Handle error in procedure error
                SqlServerErrorNumbers.ErrorInTrigger => $"Error in trigger.{sqlException.GetAllMessages()}",// Handle error in trigger error
                SqlServerErrorNumbers.ErrorInView => $"Error in view.{sqlException.GetAllMessages()}",// Handle error in view error
                SqlServerErrorNumbers.ErrorInCursor => $"Error in cursor.{sqlException.GetAllMessages()}",// Handle error in cursor error
                SqlServerErrorNumbers.ErrorInTransaction => $"Error in transaction.{sqlException.GetAllMessages()}",// Handle error in transaction error
                SqlServerErrorNumbers.ErrorInUserTransaction => $"Error in user transaction.{sqlException.GetAllMessages()}",// Handle error in user transaction error
                SqlServerErrorNumbers.ErrorInUserTransactionCommit => $"Error in user transaction commit.{sqlException.GetAllMessages()}",// Handle error in user transaction commit error
                SqlServerErrorNumbers.ErrorInUserTransactionRollback => $"Error in user transaction rollback.{sqlException.GetAllMessages()}",// Handle error in user transaction rollback error
                SqlServerErrorNumbers.ErrorInUserTransactionSavepoint => $"Error in user transaction savepoint.{sqlException.GetAllMessages()}",// Handle error in user transaction savepoint error
                SqlServerErrorNumbers.ErrorInUserTransactionRelease => $"Error in user transaction release.{sqlException.GetAllMessages()}",// Handle error in user transaction release error
                SqlServerErrorNumbers.ErrorInUserTransactionLock => $"Error in user transaction lock.{sqlException.GetAllMessages()}",// Handle error in user transaction lock error
                SqlServerErrorNumbers.ErrorInUserTransactionUnlock => $"Error in user transaction unlock.{sqlException.GetAllMessages()}",// Handle error in user transaction unlock error
                SqlServerErrorNumbers.ErrorInUserTransactionIsolationLevel => $"Error in user transaction isolation level.{sqlException.GetAllMessages()}",// Handle error in user transaction isolation level error
                SqlServerErrorNumbers.ErrorInUserTransactionDeadlockPriority => $"Error in user transaction deadlock priority.{sqlException.GetAllMessages()}",// Handle error in user transaction deadlock priority error
                SqlServerErrorNumbers.ErrorInUserTransactionLockTimeout => $"Error in user transaction lock timeout.{sqlException.GetAllMessages()}",// Handle error in user transaction lock timeout error
                SqlServerErrorNumbers.ErrorInUserTransactionLockTimeoutMs => $"Error in user transaction lock timeout in milliseconds.{sqlException.GetAllMessages()}",// Handle error in user transaction lock timeout in milliseconds error
                SqlServerErrorNumbers.ErrorInUserTransactionLockTimeoutSec => $"Error in user transaction lock timeout in seconds.{sqlException.GetAllMessages()}",// Handle error in user transaction lock timeout in seconds error
                SqlServerErrorNumbers.ErrorInUserTransactionLockTimeoutMin => $"Error in user transaction lock timeout in minutes.{sqlException.GetAllMessages()}",// Handle error in user transaction lock timeout in minutes error
                SqlServerErrorNumbers.ErrorInUserTransactionLockTimeoutHour => $"Error in user transaction lock timeout in hours.{sqlException.GetAllMessages()}",// Handle error in user transaction lock timeout in hours error
                SqlServerErrorNumbers.ErrorInUserTransactionLockTimeoutDay => $"Error in user transaction lock timeout in days.{sqlException.GetAllMessages()}",// Handle error in user transaction lock timeout in days error
                SqlServerErrorNumbers.ErrorInUserTransactionLockTimeoutWeek => $"Error in user transaction lock timeout in weeks.{sqlException.GetAllMessages()}",// Handle error in user transaction lock timeout in weeks error
                SqlServerErrorNumbers.ErrorInUserTransactionLockTimeoutMonth => $"Error in user transaction lock timeout in months.{sqlException.GetAllMessages()}",// Handle error in user transaction lock timeout in months error
                SqlServerErrorNumbers.ErrorInUserTransactionLockTimeoutYear => $"Error in user transaction lock timeout in years.{sqlException.GetAllMessages()}",// Handle error in user transaction lock timeout in years error
                SqlServerErrorNumbers.ErrorInUserTransactionLockTimeoutDecade => $"Error in user transaction lock timeout in decades.{sqlException.GetAllMessages()}",// Handle error in user transaction lock timeout in decades error
                SqlServerErrorNumbers.ErrorInUserTransactionLockTimeoutCentury => $"Error in user transaction lock timeout in centuries.{sqlException.GetAllMessages()}",// Handle error in user transaction lock timeout in centuries error
                SqlServerErrorNumbers.ErrorInUserTransactionLockTimeoutMillennium => $"Error in user transaction lock timeout in millenniums.{sqlException.GetAllMessages()}",// Handle error in user transaction lock timeout in millenniums error
                                                                                                                                                                              // Add more cases as needed
                _ => sqlException.GetAllMessages(),
            };
        }
    }
}
