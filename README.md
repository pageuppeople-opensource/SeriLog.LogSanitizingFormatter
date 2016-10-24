# SeriLog.LogSanitizingFormatter

The purpose of this package is to process and sanitize the logevent given a set of sanitizing rules.

### Getting Started

The package comes with a default processor and sanitizing rules out of the box. What the default processor and sanitizing formatter does is it extract the message template and exception details and scrub the content that contains payload or access_token.

Add the Serilog.LogSanitizingFormatter NuGet package to your project using the NuGet Package Manager or run the following command in the Package Console Window:

```
Install-Package SeriLog.LogSanitizingFormatter
```

When setting up your logger, add `.WriteTo.Console(new JsonLogSanitizingFormatter(new JsonFormatter(), true))` line ike so:

```
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using SeriLog.LogSanitizingFormatter;

namespace PageUp.JobSource.Infrastructure.Logging
{
    public class Logger
    {
        public static Serilog.Core.Logger CreateSerilogLogger()
        {
            return new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Verbose()
                .WriteTo.Console(new JsonLogSanitizingFormatter(new JsonFormatter(), true))
                .CreateLogger();
        }
    }
}
```

### Running test locally

### Custodian 

Primary custodian: [@terencet](https://github.com/terencet) <br />
Other Custodians: [@stu-mck](https://github.com/stu-mck)
