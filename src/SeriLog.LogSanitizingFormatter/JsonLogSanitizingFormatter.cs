using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace SeriLog.LogSanitizingFormatter
{
    public class JsonLogSanitizingFormatter : ITextFormatter
    {
        private readonly IProcessor _processor;
        private readonly IEnumerable<ISanitizingFormatRule> _sanitizingFormatRules;
        private readonly JsonFormatter _jsonFormatter;
        private readonly bool _sanitizeLogContent;

        /// <summary>
        /// Use your own implementation of processor and rules
        /// </summary>
        /// <param name="jsonFormatter">Json Formatter</param>
        /// <param name="sanitizeLogContent">flag to turn sanitising on or off</param>
        public JsonLogSanitizingFormatter(IProcessor processor, IEnumerable<ISanitizingFormatRule> sanitizingFormatRules, JsonFormatter jsonFormatter, bool sanitizeLogContent)
        {
            _processor = processor;
            _sanitizingFormatRules = sanitizingFormatRules;
            _jsonFormatter = jsonFormatter;
            _sanitizeLogContent = sanitizeLogContent;            
        }

        /// <summary>
        /// Use default processor with your own rules
        /// </summary>
        /// <param name="jsonFormatter">Json Formatter</param>
        /// <param name="sanitizeLogContent">flag to turn sanitising on or off</param>
        public JsonLogSanitizingFormatter(IEnumerable<ISanitizingFormatRule> saninitizerRules, JsonFormatter jsonFormatter, bool sanitizeLogContent)
            : this(new DefaultProcessor(), saninitizerRules, jsonFormatter, sanitizeLogContent)
        {
        }

        /// <summary>
        /// Use default sanitizing formatting rule with default processor
        /// </summary>
        /// <param name="jsonFormatter">Json Formatter</param>
        /// <param name="sanitizeLogContent">flag to turn sanitising on or off</param>
        public JsonLogSanitizingFormatter(JsonFormatter jsonFormatter, bool sanitizeLogContent)
            : this(SanitizerHelper.SanitizingFormatRules, jsonFormatter, sanitizeLogContent)
        {
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            if (_sanitizeLogContent)
                Sanitize(logEvent, output);
            else
                _jsonFormatter.Format(logEvent, output);
        }

        private void Sanitize(LogEvent logEvent, TextWriter output)
        {
            var tempTextWriter = new StringWriter();

            _jsonFormatter.Format(logEvent, tempTextWriter);

            var jsonObject = JsonConvert.DeserializeObject<dynamic>(tempTextWriter.GetStringBuilder().ToString());

            var processedLogEvent = _processor.Process(jsonObject, _sanitizingFormatRules);

            output.WriteLine(processedLogEvent);
        }
    }
}