using Core.Config;
using Core.Interfaces;
using Discord_Bot;
using dotenv.net;

namespace CommandLine;

public class Program(ILogger logger, IDiscordBot discordBot) : IProcess
{

    public static async Task Main()
    {
        // Initialize the logger and Discord bot
        var logger = new Logger();
        var discordBot = new DiscordBot(logger);
        // Login to Discord using the token from environment variables
        DotEnv.Load();
        string? token = EnvironmentVars.DiscordBotToken;
        if (string.IsNullOrEmpty(token)) throw new Exception("Token not found");
        await discordBot.LoginAsync(token);
        // Create an instance of the Program class
        var program = new Program(logger, discordBot);
        await program.StartAsync();

        await Task.Delay(-1);
    }

    public async Task StartAsync()
    {
        await discordBot.StartAsync();
    }

    public async Task StopAsync()
    {
        await discordBot.StopAsync();
    }
}