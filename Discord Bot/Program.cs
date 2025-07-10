using Core.Config;
using Core.Services;
using dotenv.net;

namespace Discord_Bot;

public static class Program
{

    public static async Task Main(string[] args)
    {
        // Initialize the logger and Discord bot
        var bot = Services.Get<Bot>();
        var loggerService = Services.Get<LoggerService>();
        loggerService.OnLog += (time, source, severity, message) =>
        {
            Console.WriteLine($"{time:hh:mm:ss} [{source}] {severity}: {message}");
        };
        loggerService.Info("Program", "Initializing Discord Bot...");
        // Login to Discord using the token from environment variables
        DotEnv.Load();
        string? token = EnvironmentVars.DiscordBotToken;
        if (string.IsNullOrEmpty(token)) throw new Exception("Token not found");
        await bot.LoginAsync(token);
        // Start the bot
        await bot.StartAsync();
        // Wait indefinitely
        await Task.Delay(Timeout.Infinite);
    }

}