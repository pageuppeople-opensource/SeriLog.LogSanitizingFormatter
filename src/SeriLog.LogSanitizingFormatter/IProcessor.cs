using System.Collections.Generic;

namespace SeriLog.LogSanitizingFormatter
{
    public interface IProcessor
    {
        dynamic Process(dynamic jsonObject, IEnumerable<ISanitizingFormatRule> rules);
    }
}