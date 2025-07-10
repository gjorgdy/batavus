using System.Globalization;
using Discord;

namespace Discord_Bot.Utils;

public static class EmbedUtils
{

    public static Embed CreateErrorEmbed(string title, string description, int statusCode = 0)
    {
        return CreateBasicEmbed(statusCode == 0 ? title : title + " " + statusCode, description, Color.Red);
    }

    public static Embed CreateBasicEmbed(string title, string description, Color color)
    {
        return new EmbedBuilder()
            .WithTitle(title)
            .WithDescription(description)
            .WithColor(color)
            .WithFooter("Batavus Bot")
            .Build();
    }

    public static uint GetColor(string colorString)
    {
        return uint.Parse(colorString.Replace('#', ' '), NumberStyles.HexNumber);
    }

}