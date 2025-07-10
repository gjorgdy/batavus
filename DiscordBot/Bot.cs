using Core.Services;
using Discord_Bot.Modules;
using Discord_Bot.Modules.MarvelRivals;
using Discord;
using Discord.WebSocket;

namespace Discord_Bot;

public class Bot(LoggerService logger, DiscordSocketClient client, InteractionHandler interactionHandler)
{

    private readonly List<SocketChannel> _logChannels = [];

    public async Task StartAsync()
    {
        // Register the interaction handler with the client
        client.Ready += async () =>
        {
            await interactionHandler.RegisterInteractions();
            client.InteractionCreated += interactionHandler.OnInteraction;
        };
        // Register the log channels
        client.Log += lm =>
        {
            if (lm.Exception != null)
            {
                logger.Log(lm.Source, lm.Severity.ToString(), $"{lm.Message} - {lm.Exception.Message}");
                return Task.CompletedTask;
            }
            logger.Log(lm.Source, lm.Severity.ToString(), lm.Message);
            return Task.CompletedTask;
        };
        // Log messages to the log channels
        logger.OnLog += async (time, source, severity, message) =>
        {
            string constructedMessage = $"{time:hh:mm:ss} ``[{source}]`` {severity}: {message}";
            foreach (var channel in _logChannels)
            {
                if (channel is SocketTextChannel textChannel) await textChannel.SendMessageAsync(constructedMessage);
            }
        };
        // Register the log channels when the bot is ready
        client.GuildAvailable += (guild) =>
        {
            foreach (var channel in guild.Channels)
            {
                if (channel is SocketTextChannel && channel.Name == "log") _logChannels.Add(channel);
            }
            return Task.CompletedTask;
        };

        client.ButtonExecuted += async (ctx) =>
        {
            await Services.Get<StatsModule>().ButtonHandler(ctx);
        };
        // Start the client
        await client.StartAsync();
    }

    public Task StopAsync() => client.StopAsync();

    public Task LoginAsync(string token) => client.LoginAsync(TokenType.Bot, token);

}