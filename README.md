# SeriLog.LogSanitizingFormatter

The purpose of this package is to process and sanitize the logevent given a set of sanitizing rules. 
The code consists of an overridable processor and a set of default sanitzing rules that are also overridable;

By default, it is designed to work in conjunction with the Serilog JsonFormatter.

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

namespace PageUp.Infrastructure.Logging
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

### Implementing a custom sanitzer
Custom sanitizers implement the ISanitizingFormatRule interface. This interface has one method, Sanitize, which will take the content of the log packet and execute it's logic over the that input.

```

using SeriLog.LogSanitizingFormatter;

namespace MySanitizingRules.MyNamespace
{
    public class DemoSanitzingRule : ISanitizingFormatRule
    {
        public string Sanitize(string content)
        {
            return content.ToLower().Replace("cat", "dog");
        }
    }
}

```

The implemented set of custom rules can be added to the sanitzer during the configuration step.

```

using System.Collections.Generic;
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
            var customRules = new List<ISanitizingFormatRule> {new DemoSanitzingRule()};

            return new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Verbose()
                .WriteTo.Console(new JsonLogSanitizingFormatter(customRules, new JsonFormatter(), true))
                .CreateLogger();
        }
    }
}

```

Rules are processed in the order they declared in the list and providing a custom list will override the default list of rules.



### Working with the JsonFormatter
Serilog's JsonFormatter generates data of a known structure and the default processor knows how to iterate this data structure. It is possible to provide your own processing logic - essentially, your own message parsing logic.

In order to implement this, implement the IProcessor interface and inject it into the sanitizer at construction time.

```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SeriLog.LogSanitizingFormatter;

namespace PageUp.JobSource.Infrastructure.Logging
{
    public class processor : IProcessor
    {
        
        public dynamic Process(dynamic jsonObject, IEnumerable<ISanitizingFormatRule> rules)
        {
            // custom logic for iterating through my message structure and applying my rules as required. 
			// the jsonObject is the message from SeriLog
			// what is returned is the final message
        }
    }
}


```


### Running test locally

### Custodian 

Primary custodian: [@terencet](https://github.com/terencet) <br />
Other Custodians: [@stu-mck](https://github.com/stu-mck)
