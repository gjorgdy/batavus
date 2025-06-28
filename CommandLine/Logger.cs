using Core.Interfaces;

namespace CommandLine;

public class Logger : ILogger
{
    public Task Log(int severity, string message)
    {
        Console.Out.WriteLine(severity + " | " + message);
        return Task.CompletedTask;
    }
}