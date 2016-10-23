using System;

namespace SeriLog.LogSanitizingFormatter.SanitizingFormatRules
{
    public class PayloadSanitizingFormatRule : ISanitizingFormatRule
    {
        public string Sanitize(string content)
        {
            var start = content.IndexOf("payload", StringComparison.CurrentCultureIgnoreCase);

            if (start > 0)
            {
                content = content.Substring(0, start) + "payload: [hidden]";
            }

            return content;
        }
    }
}