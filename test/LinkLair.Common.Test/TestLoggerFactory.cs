using Microsoft.Extensions.Logging;

namespace LinkLair.Common.Test;

public static class TestLoggerFactory
{
    public static ILoggerFactory Create()
    {
        return new LoggerFactory();
    }

    public static ILogger CreateLogger(string name)
    {
        return Create().CreateLogger(name);
    }
}
