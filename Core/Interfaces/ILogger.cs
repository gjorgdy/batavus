namespace Core.Interfaces;

public interface ILogger
{

    Task Log(int severity, string message);

}