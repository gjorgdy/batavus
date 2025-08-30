using CoreModules.Stats.MarvelRivals.Models;
using CoreModules.Stats.MarvelRivals.Responses;
using Discord_Bot.Utils;
using Discord;

namespace Discord_Bot.Modules.MarvelRivals;

public static class MarvelRivalsEmbedFactory
{
    public enum Pages
    {
        GeneralStats,
        Teammates,
        Unranked,
        Ranked
    }

    public static MessageComponent BuildComponents(MarvelRivalsPlayer player, Pages page)
    {
        bool isMainStats = page == Pages.GeneralStats;
        bool isTeammates = page == Pages.Teammates;
        bool isUnranked = page == Pages.Unranked;
        bool isRanked = page == Pages.Ranked;

        return new ComponentBuilder()
            .WithButton("Main Stats", $"smr_stats_{player.Data.Uid}", isMainStats ? ButtonStyle.Primary : ButtonStyle.Secondary, disabled: isMainStats)
            .WithButton("Teammates", $"smr_teammates_{player.Data.Uid}", isTeammates ? ButtonStyle.Primary : ButtonStyle.Secondary, disabled: isTeammates)
            .WithButton("Unranked", $"smr_unranked_{player.Data.Uid}", isUnranked ? ButtonStyle.Primary : ButtonStyle.Secondary, disabled: isUnranked)
            .WithButton("Ranked", $"smr_ranked_{player.Data.Uid}", isRanked ? ButtonStyle.Primary : ButtonStyle.Secondary, disabled: isRanked)
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

    public static Embed BuildGeneralStatsPage(MarvelRivalsPlayer player)
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
            .AddField("MVPs", player.Stats.TotalMvpCount, true)
            .AddField("SVPs", player.Stats.TotalSvpCount, true)
            .AddField("KDA", $"{player.Stats.AverageKda:F}", true)
            .AddField("Winrate", $"{player.Stats.AverageWinRate:F}%", true);

        return embedBuilder.Build();
    }

    public static Embed BuildUnrankedStatsPage(MarvelRivalsPlayer player)
    {
        return BuildStatsPage(player.Stats.Unranked, BaseEmbedBuilder(player, "unranked stats"));
    }

    public static Embed BuildRankedStatsPage(MarvelRivalsPlayer player)
    {
        return BuildStatsPage(player.Stats.Ranked, BaseEmbedBuilder(player, "ranked stats"));
    }

    private static Embed BuildStatsPage(ApiPlayerResponse.StatsSection stats, EmbedBuilder embedBuilder)
    {
        embedBuilder = embedBuilder
            // KDA
            .AddField("Kills", stats.Kills, true)
            .AddField("Deaths", stats.Deaths, true)
            .AddField("Assists", stats.Assists, true)
            // Averages
            .AddField("KDA", $"{stats.Kda:F}", true)
            .AddField("MVPs", stats.MvpCount, true)
            .AddField("SVPs", stats.SvpCount, true)
            // Matches and Winrate
            .AddField("Matches", stats.Matches, true)
            .AddField("Wins", stats.Wins, true)
            .AddField("Winrate", $"{stats.WinRate:F}%", true)
            // Time played
            .AddField("Time Played", $"{stats.TimePlayed.Hours}h {stats.TimePlayed.Minutes}m {stats.TimePlayed.Seconds}s", true);

        return embedBuilder.Build();
    }

    public static Embed BuildTeammatesPage(MarvelRivalsPlayer player)
    {
        var embedBuilder = BaseEmbedBuilder(player, "teammates");
        if (player.Teammates.Length > 0)
        {
            // filter only if any common teammates
            var bestTeamMate = player.Teammates.Any(tm => tm.Matches > 5)
                ? player.Teammates.Where(tm => tm.Matches > 5).MaxBy(tm => tm.WinRate)
                : player.Teammates.MaxBy(tm => tm.WinRate);

            embedBuilder = embedBuilder
                .AddField("Best Team Mate", bestTeamMate.PlayerData.Name, true)
                .AddField("Matches", bestTeamMate.Matches, true)
                .AddField("Win Rate", $"{bestTeamMate.WinRate:F}%", true);

            var mostCommonTeamMate = player.Teammates
                .MaxBy(tm => tm.Matches);
            embedBuilder = embedBuilder
                .AddField("Most Common Team Mate", mostCommonTeamMate.PlayerData.Name, true)
                .AddField("Matches", mostCommonTeamMate.Matches, true)
                .AddField("Win Rate", $"{mostCommonTeamMate.WinRate:F}%", true);
        }
        else
        {
            embedBuilder.AddField("No teammates found", "Try searching for a different player.");
        }
        return embedBuilder.Build();
    }

}