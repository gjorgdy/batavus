using static System.UInt64;

namespace Core.Config;

public static class EnvironmentVars
{
    public static string? DiscordBotToken => Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");
    public static ulong? DiscordDevGuildId  {
        get
        {
            bool tryParse = TryParse(Environment.GetEnvironmentVariable("DISCORD_DEV_GUILD_ID"), out ulong guildId);
            return tryParse ? guildId : null;
        }
    }
    public static string? MarvelRivalsApiKey => Environment.GetEnvironmentVariable("MARVEL_RIVALS_API_KEY");
}