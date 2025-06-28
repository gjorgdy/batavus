namespace Core.Interfaces;

public interface IProcess
{
    Task StartAsync();
    Task StopAsync();
}