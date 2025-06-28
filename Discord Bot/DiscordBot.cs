using Core.Interfaces;
using Discord;
using Discord.WebSocket;

namespace Discord_Bot;

public class DiscordBot : IDiscordBot
{
    private readonly DiscordSocketClient _client;
    private readonly List<SocketChannel> _logChannels = [];

    public DiscordBot(ILogger logger)
    {
        _client = new DiscordSocketClient();
        _client.Log += Log;
        _client.Log += (lm) => logger.Log(lm.Severity.GetHashCode(), lm.ToString());
    }

    public async Task StartAsync()
    {
        await _client.StartAsync();

        _client.Ready += () =>
        {
            var channel = _client.GetChannel(ulong.Parse("1388548772715565107"));
            if (channel != null) _logChannels.Add(channel);
            return Task.CompletedTask;
        };
    }

    public Task StopAsync() => _client.StopAsync();

    public Task LoginAsync(string token) => _client.LoginAsync(TokenType.Bot, token);

    private async Task Log(LogMessage msg)
    {
        string prefix = msg.Severity switch
        {
            LogSeverity.Critical => "🔥",
            LogSeverity.Error => "⛓️‍💥",
            LogSeverity.Warning => "⚠️‍",
            LogSeverity.Info => "ℹ️‍",
            LogSeverity.Verbose => "🔍‍",
            LogSeverity.Debug => "🐛‍",
            _ => "??"
        };
        foreach (var channel in _logChannels)
        {
            if (channel is SocketTextChannel textChannel)
                await textChannel.SendMessageAsync(prefix + " ``" + msg.ToString() + "``");
        }
    }

}