using Microsoft.AspNetCore.Identity;
using System.Text;

namespace StartTemplateNew.Shared.Helpers
{
    public static class IdentityHelper
    {
        public static string GetErrors(this IdentityResult result)
        {
            StringBuilder sb = new();
            foreach (IdentityError error in result.Errors)
            {
                sb.AppendLine(error.Description);
            }

            return sb.ToString();
        }
    }
}
