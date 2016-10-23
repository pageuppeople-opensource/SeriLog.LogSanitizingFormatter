using SeriLog.LogSanitizingFormatter.SanitizingFormatRules;
using System.Collections.Generic;

namespace SeriLog.LogSanitizingFormatter
{
    public static class SanitizerHelper
    {
        /// <summary>
        /// A list of default sanitizing formating rules
        /// </summary>
        public static readonly ISanitizingFormatRule[] SanitizingFormatRules =
        {
            new PayloadSanitizingFormatRule(),
            new AccessTokenSanitizingFormatRule(),
        };

        /// <summary>
        /// Purpose of the method is to take a content string, apply every rule to the content
        /// </summary>
        /// <param name="sanitizingFormatRules">A list of sanitizing formatting rules</param>
        /// <param name="content">content to sanitize</param>
        /// <returns>sanitized content</returns>
        public static string Sanitize(IEnumerable<ISanitizingFormatRule> sanitizingFormatRules, object content)
        {
            var sanitizedContent = content.ToString();
            foreach (var sanitizingFormatRule in sanitizingFormatRules)
                sanitizedContent = sanitizingFormatRule.Sanitize(sanitizedContent);

            return sanitizedContent;
        }
    }
}