using System.Text;

namespace StartTemplateNew.Shared.ExceptionHelpers.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetAllMessages(this Exception ex)
        {
            StringBuilder sb = new();
            Exception? currentException = ex;

            while (currentException != null)
            {
                sb.AppendLine(currentException.Message);
                currentException = currentException.InnerException;
            }

            return sb.ToString();
        }
    }
}
