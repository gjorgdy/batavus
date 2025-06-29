using Core.Config;
using Core.Interfaces;
using Core.Services;
using dotenv.net;

namespace CommandLine;

public class Program : IProcess
{
    private LoggerService Logger { get; } = Services.Get<LoggerService>();
    private IDiscordBot DiscordBot { get; } = Services.Get<IDiscordBot>();

    public static async Task Main()
    {
        // Initialize the logger and Discord bot
        var discordBot = Services.Get<IDiscordBot>();
        // Login to Discord using the token from environment variables
        DotEnv.Load();
        string? token = EnvironmentVars.DiscordBotToken;
        if (string.IsNullOrEmpty(token)) throw new Exception("Token not found");
        await discordBot.LoginAsync(token);
        // Create an instance of the Program class
        var program = new Program();

        await program.StartAsync();
    }

    public Task StartAsync()
    {
        Start();
        return Task.CompletedTask;
    }

    public async Task StopAsync()
    {
        await DiscordBot.StopAsync();
    }

    private void Start()
    {
        // Register the logger service
        Services.Get<LoggerService>().OnLog += (time, source, severity, message) =>
        {
            Console.Out.WriteLine(time.TimeOfDay + " [" + severity + "] " + source + ": " + message);
        };
        // Start background tasks
        DiscordBot.StartAsync();
        // Start console input loop
        while (true)
        {
            string? input = Console.ReadLine();
            if (input == null) break;
            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting...");
                break;
            }
            if (input.StartsWith("log", StringComparison.OrdinalIgnoreCase))
            {
                string message = input.Replace("log ", "");
                Logger.Verbose(this, message);
            }
            else
            {
                Console.WriteLine($"You entered: {input}");
            }
        }
    }
}