using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace pickuphockey.Functions
{
    public class ForwardingLoggerProvider(ILoggerFactory loggerFactory) : ILoggerProvider
    {
        private readonly ILoggerFactory _loggerFactory = loggerFactory;

        public ILogger CreateLogger(string categoryName)
        {
            return _loggerFactory.CreateLogger(categoryName);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    public class InMemoryLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, InMemoryLogger> _loggers = new();

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new InMemoryLogger(name));
        }

        public void Dispose()
        {
            _loggers.Clear();
            GC.SuppressFinalize(this);
        }

        public string GetLogs()
        {
            return string.Join(Environment.NewLine, _loggers.Values.SelectMany(l => l.Logs));
        }
    }

    public class InMemoryLogger(string name) : ILogger
    {
        private readonly string _name = name;
        public List<string> Logs { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);
            var logEntry = $"{logLevel.ToString()[0]}: {message}";
            Logs.Add(logEntry);
        }
    }
}
