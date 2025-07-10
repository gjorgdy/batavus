using CoreModules.Stats.MarvelRivals.Models;
using Discord_Bot.Utils;
using Discord;

namespace Discord_Bot.Modules.MarvelRivals;

public static class MarvelRivalsEmbedFactory
{
    public enum Pages
    {
        MainStats,
        Teammates,
        Unranked,
        Ranked
    }

    public static MessageComponent BuildComponents(MarvelRivalsPlayer player, Pages page)
    {
        return new ComponentBuilder()
            .WithButton("Main Stats", $"smr_stats_{player.Data.Uid}", page == Pages.MainStats ? ButtonStyle.Primary : ButtonStyle.Secondary)
            .WithButton("Teammates", $"smr_teammates_{player.Data.Uid}", page == Pages.Teammates ? ButtonStyle.Primary : ButtonStyle.Secondary)
            .WithButton("tracker.gg", style: ButtonStyle.Link, url: $"https://tracker.gg/marvel-rivals/profile/ign/{player.Data.Name}")
            .Build();
    }

    private static EmbedBuilder BaseEmbedBuilder(MarvelRivalsPlayer player, string pageTitle)
    {
        var embedBuilder = new EmbedBuilder()
            .WithTitle($"{player.Data.Name}'s {pageTitle}")
            .WithThumbnailUrl(player.Data.Icon.Url)
            .WithFooter($"last updated: {player.Updates.Last?.ToString("dd/MM/yyyy HH:mm") ?? "unknown"}");
        return player.Data.Rank.Name != "Invalid level"
            ? embedBuilder.WithColor(EmbedUtils.GetColor(player.Data.Rank.Color))
            : embedBuilder;
    }

    public static Embed BuildMainStatsPage(MarvelRivalsPlayer player)
    {
        var embedBuilder = BaseEmbedBuilder(player, "stats")
            .AddField("Team", player.Data.Team.Name, true)
            .AddField("Level", player.Data.Level, true);
        // Add a rank field if the rank is valid
        if (player.Data.Rank.Name != "Invalid level")
        {
            embedBuilder = embedBuilder
                .AddField("Rank", player.Data.Rank.Name, true)
                .WithImageUrl(player.Data.Rank.Url);
        }
        // Add stats
        embedBuilder = embedBuilder
            .AddField("Winrate", $"{player.Stats.AverageWinRate:F}%", true)
            .AddField("MVPs", player.Stats.TotalMvpCount, true)
            .AddField("SVPs", player.Stats.TotalSvpCount, true)
            .AddField("KDA", $"{player.Stats.AverageKda:F}", true);

        return embedBuilder.Build();
    }

    public static Embed BuildTeammatesPage(MarvelRivalsPlayer player)
    {
        var embedBuilder = BaseEmbedBuilder(player, "teammates");
        if (player.Teammates.Length > 0)
        {
            var bestTeamMate = player.Teammates
                .Where(tm => tm.Matches > 5)
                .MaxBy(tm => tm.WinRate);
            embedBuilder = embedBuilder
                .AddField("Best Team Mate", bestTeamMate.PlayerData.Name, true)
                .AddField("Matches", bestTeamMate.Matches, true)
                .AddField("Win Rate", (bestTeamMate.WinRate / 100) + "%", true);

            var mostCommonTeamMate = player.Teammates
                .MaxBy(tm => tm.Matches);
            embedBuilder = embedBuilder
                .AddField("Most Common Team Mate", mostCommonTeamMate.PlayerData.Name, true)
                .AddField("Matches", mostCommonTeamMate.Matches, true)
                .AddField("Win Rate", (mostCommonTeamMate.WinRate / 100) + "%", true);
        }
        else
        {
            embedBuilder.AddField("No teammates found", "Try searching for a different player.");
        }
        return embedBuilder.Build();
    }

}