using Core.Interfaces;
using Core.Services;
using Discord;
using Discord.WebSocket;

namespace Discord_Bot;

public class DiscordBot : IDiscordBot
{
    private readonly DiscordSocketClient _client;
    private readonly List<SocketChannel> _logChannels = [];

    public DiscordBot(LoggerService logger)
    {
        _client = new DiscordSocketClient();
        _client.Log += lm => logger.Log(lm.Source, lm.Severity.ToString(), lm.Message);
        logger.OnLog += async (time, source, severity, message) =>
        {
            string constructedMessage = time.TimeOfDay + " ``" + source + " > " + severity + "`` " + message;
            foreach (var channel in _logChannels)
            {
                if (channel is SocketTextChannel textChannel) await textChannel.SendMessageAsync(constructedMessage);
            }
        };
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

}