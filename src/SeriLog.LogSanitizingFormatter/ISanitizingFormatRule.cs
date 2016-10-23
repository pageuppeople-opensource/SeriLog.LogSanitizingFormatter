namespace SeriLog.LogSanitizingFormatter
{
    public interface ISanitizingFormatRule
    {
        string Sanitize(string content);
    }
}