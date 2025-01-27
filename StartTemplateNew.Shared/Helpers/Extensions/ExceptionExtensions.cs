using System.Text;

namespace StartTemplateNew.Shared.Helpers.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetFullMessage(this Exception ex)
        {
            StringBuilder sb = new();
            sb.AppendLine(ex.Message);

            Exception? inner = ex.InnerException;
            while (inner != null)
            {
                sb.AppendLine(inner.Message);
                inner = inner.InnerException;
            }

            return sb.ToString();
        }
    }
}
