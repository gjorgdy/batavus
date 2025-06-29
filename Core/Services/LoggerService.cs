namespace Core.Services;

public class LoggerService
{
    public delegate void LogHandler(DateTime time, string source, string severity, string message);

    public event LogHandler? OnLog;

    public Task Log(string source, string severity, string message)
    {
        OnLog?.Invoke(DateTime.Now, source, severity, message);
        return Task.CompletedTask;
    }

    private Task Log(object source, string severity, string message)
    {
        string sourceName = source as string
            ?? source.GetType().ToString().Split(".").Last();
        return Log(sourceName, severity, message);
    }

    public void CriticalError(object sender, string message) => Log(sender, "Critical", message);
    public void Error(object sender, string message) => Log(sender, "Error", message);
    public void Warning(object sender, string message) => Log(sender, "Warning", message);
    public void Info(object sender, string message) => Log(sender, "Info", message);
    public void Verbose(object sender, string message) => Log(sender, "Verbose", message);
    public void Debug(object sender, string message) => Log(sender, "Debug", message);

}