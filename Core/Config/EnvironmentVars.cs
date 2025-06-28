namespace Core.Config;

public static class EnvironmentVars
{
    public static string? DiscordBotToken => Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");
}