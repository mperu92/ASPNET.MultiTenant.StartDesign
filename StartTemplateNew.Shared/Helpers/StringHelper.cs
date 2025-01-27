using StartTemplateNew.Shared.Helpers.Const;

namespace StartTemplateNew.Shared.Helpers
{
    public static class StringHelper
    {
        public static bool EqualsMany(this string text, IEnumerable<string> values, StringComparison stringComparison = StringComparison.Ordinal)
        {
            ArgumentException.ThrowIfNullOrEmpty(text);

            if (values?.Any() != true)
                return false;

            return values.Any(value => text.Equals(value, stringComparison));
        }

        public static bool AreNullOrWhiteSpace(params string[] values)
        {
            return values.All(string.IsNullOrWhiteSpace);
        }

        public static bool AreNullOrEmpty(params string[] values)
        {
            return values.All(string.IsNullOrEmpty);
        }

        public static string GenerateAlphaNumericString(int length = 10, bool toUpperCase = false, string? prefix = null, string? suffix = null)
        {
            int suffixPrefixLength = (prefix?.Length + suffix?.Length) ?? 0;
            if (length <= suffixPrefixLength)
                throw new ArgumentException("Length must be greater than the sum of the prefix and suffix lengths.");

            int charsToGenerate = length - suffixPrefixLength;
            char[] stringChars = new char[charsToGenerate];
            Random random = new();
            for (int i = 0; i < charsToGenerate; i++)
            {
                stringChars[i] = StringDefaults.AlphaNumericChars[random.Next(StringDefaults.AlphaNumericChars.Length)];
            }

            string finalString = new(stringChars);
            if (!string.IsNullOrEmpty(prefix))
                finalString = prefix + finalString;
            if (!string.IsNullOrEmpty(suffix))
                finalString += suffix;

            if (toUpperCase)
                finalString = finalString.ToUpperInvariant();

            return finalString;
        }

        public static string ToNiceUrl(this string text, int length = 20)
        {
            text = text.ToLowerInvariant();
            text = text.Replace("-", string.Empty);
            text = text.Replace(' ', '-');
            text = text.Replace("  ", "-");
            text = text.Replace("   ", "-");
            text = text.Replace("\n", "-");
            text = text.Replace(Environment.NewLine, "-");
            text = text.Replace("ç", "c");
            text = text.Replace("ğ", "g");
            text = text.Replace("ı", "i");
            text = text.Replace("ö", "o");
            text = text.Replace("ş", "s");
            text = text.Replace("ü", "u");
            text = text.Replace("à", "a");
            text = text.Replace("è", "e");
            text = text.Replace("é", "e");
            text = text.Replace("ì", "i");
            text = text.Replace("ò", "o");
            text = text.Replace("ù", "u");
            text = text.Replace("?", string.Empty);
            text = text.Replace("!", string.Empty);
            text = text.Replace(".", string.Empty);
            text = text.Replace(",", string.Empty);
            text = text.Replace(";", string.Empty);
            text = text.Replace(":", string.Empty);
            text = text.Replace("(", string.Empty);
            text = text.Replace(")", string.Empty);
            text = text.Replace("[", string.Empty);
            text = text.Replace("]", string.Empty);
            text = text.Replace("{", string.Empty);
            text = text.Replace("}", string.Empty);
            text = text.Replace("<", string.Empty);
            text = text.Replace(">", string.Empty);
            text = text.Replace("=", string.Empty);
            text = text.Replace("+", string.Empty);
            text = text.Replace("*", string.Empty);
            text = text.Replace("/", string.Empty);
            text = text.Replace("\\", string.Empty);
            text = text.Replace("|", string.Empty);
            text = text.Replace("&", string.Empty);
            text = text.Replace("%", string.Empty);
            text = text.Replace("#", string.Empty);
            text = text.Replace("@", string.Empty);
            text = text.Replace("$", string.Empty);
            text = text.Replace("€", string.Empty);
            text = text.Replace("£", string.Empty);
            text = text.Replace("¥", string.Empty);
            text = text.Replace("₺", string.Empty);
            text = text.Replace("₽", string.Empty);
            text = text.Replace("₿", string.Empty);
            text = text.Replace("₹", string.Empty);
            text = text.Replace("₸", string.Empty);
            text = text.Replace("₴", string.Empty);
            text = text.Replace("₡", string.Empty);
            text = text.Replace("₢", string.Empty);
            text = text.Replace("₣", string.Empty);
            text = text.Replace("₤", string.Empty);
            text = text.Replace("₥", string.Empty);
            text = text.Replace("₦", string.Empty);
            text = text.Replace("₧", string.Empty);
            text = text.Replace("₨", string.Empty);
            text = text.Replace("₩", string.Empty);
            text = text.Replace("₪", string.Empty);
            text = text.Replace("₫", string.Empty);
            text = text.Replace("₭", string.Empty);
            text = text.Replace("₮", string.Empty);
            text = text.Replace("₯", string.Empty);
            text = text.Replace("₰", string.Empty);
            text = text.Replace("₱", string.Empty);
            text = text.Replace("₲", string.Empty);
            text = text.Replace("₳", string.Empty);
            text = text.Replace("₵", string.Empty);
            text = text.Replace("₶", string.Empty);
            text = text.Replace("₷", string.Empty);
            text = text.Replace("₻", string.Empty);
            text = text.Replace("₼", string.Empty);
            text = text.Replace("₾", string.Empty);
            text = text.Replace("₠", string.Empty);

            if (text.Length > length)
                text = text[..length];

            if (text.EndsWith('-'))
                text = text[..^1];

            return text;
        }
    }
}
