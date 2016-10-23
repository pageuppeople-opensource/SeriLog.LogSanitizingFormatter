using System.Collections.Generic;

namespace SeriLog.LogSanitizingFormatter
{
    public class DefaultProcessor : IProcessor
    {
        public dynamic Process(dynamic jsonObject, IEnumerable<ISanitizingFormatRule> rules)
        {
            // scrub message template
            if (jsonObject.MessageTemplate != null)
                jsonObject.MessageTemplate = SanitizerHelper.Sanitize(rules, jsonObject.MessageTemplate);

            if (jsonObject.Exception == null) return jsonObject;

            // scrub root exception
            jsonObject.Exception = SanitizerHelper.Sanitize(rules, jsonObject.Exception);

            var detail = jsonObject?.Properties?.ExceptionDetail;

            if (detail == null) return jsonObject;

            // scrub exception message
            detail.Message = SanitizerHelper.Sanitize(rules, detail.Message);

            if (detail.InnerException != null)
                CleanInnerExceptions(detail.InnerException, rules);

            if (detail.InnerExceptions == null) return jsonObject;

            // scrub inner exceptions recursively
            var exceptions = (List<dynamic>)detail.InnerExceptions;
            foreach (var exception in exceptions)
            {
                CleanInnerExceptions(exception, rules);
            }

            return jsonObject;
        }

        public dynamic Process(dynamic jsonObject)
        {
            var rules = SanitizerHelper.SanitizingFormatRules;

            // scrub message template
            if (jsonObject.MessageTemplate != null)
                jsonObject.MessageTemplate = SanitizerHelper.Sanitize(rules, jsonObject.MessageTemplate);

            if (jsonObject.Exception == null) return jsonObject;

            // scrub root exception
            jsonObject.Exception = SanitizerHelper.Sanitize(rules, jsonObject.Exception);

            var detail = jsonObject?.Properties?.ExceptionDetail;

            if (detail == null) return jsonObject;

            // scrub exception message
            detail.Message = SanitizerHelper.Sanitize(rules, detail.Message);

            if (detail.InnerException != null)
                CleanInnerExceptions(detail.InnerException, rules);

            if (detail.InnerExceptions == null) return jsonObject;

            // scrub inner exceptions recursively
            var exceptions = (List<dynamic>)detail.InnerExceptions;
            foreach (var exception in exceptions)
            {
                CleanInnerExceptions(exception, rules);
            }

            return jsonObject;
        }

        private static void CleanInnerExceptions(dynamic innerException, IEnumerable<ISanitizingFormatRule> rules)
        {
            if (innerException == null || innerException.Message == null) return;

            innerException.Message = SanitizerHelper.Sanitize(rules, innerException.Message);

            if (innerException.InnerException != null)
                CleanInnerExceptions(innerException.InnerException, rules);
        }
    }
}
