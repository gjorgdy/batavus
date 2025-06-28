namespace Core.Interfaces;

public interface IDiscordBot : IProcess
{
    Task LoginAsync(string token);
}