using System.Text.RegularExpressions;

namespace SeriLog.LogSanitizingFormatter.SanitizingFormatRules
{
    public class AccessTokenSanitizingFormatRule : ISanitizingFormatRule
    {
        private static readonly Regex AccessTokenRegex = new Regex(@"\{.*(access_token).*\}");

        public string Sanitize(string content)
        {
            if (content.ToLower().Contains("access_token"))
            {
                content = AccessTokenRegex.Replace(content, "[hidden]");
            }

            return content;
        }
    }
}