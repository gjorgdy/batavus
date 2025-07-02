using Discord;

namespace Discord_Bot.Utils;

public static class EmbedUtils
{

    public static Embed CreateErrorEmbed(string title, string description)
    {
        return CreateBasicEmbed(title, description, Color.Red);
    }

    public static Embed CreateBasicEmbed(string title, string description, Color color)
    {
        return new EmbedBuilder()
            .WithTitle(title)
            .WithDescription(description)
            .WithColor(color)
            .Build();
    }

}