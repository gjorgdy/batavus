using Newtonsoft.Json;

namespace CoreModules.Stats.MarvelRivals.Responses;

public class ApiPlayerResponse
{

    [JsonProperty("updates")]
    public UpdateSection Updates { get; init; }

    public readonly struct UpdateSection
    {
        [JsonProperty("last_update_request")]
        public DateTime? Last { get; init; }
    }

    [JsonProperty("player")]
    public required PlayerSection Data { get; init; }

    [JsonProperty("team_mates")]
    public required TeammateSection[] Teammates { get; init; }

    [JsonProperty("overall_stats")]
    public required OverallStatsSection Stats { get; init; }

    public readonly struct OverallStatsSection
    {
        [JsonIgnore]
        public double AverageWinRate =>
            TotalMatches > 0 ? (double) TotalWins / TotalMatches * 100 : 0;

        [JsonIgnore]
        public double AverageKda => (Unranked.Kda + Ranked.Kda) / 2;

        [JsonIgnore]
        public double TotalMvpCount => Unranked.MvpCount + Ranked.MvpCount;

        [JsonIgnore]
        public double TotalSvpCount => Unranked.SvpCount + Ranked.SvpCount;

        [JsonProperty("total_matches")]
        public required int TotalMatches { get; init; }

        [JsonProperty("total_wins")]
        public required int TotalWins { get; init; }

        [JsonProperty("unranked")]
        public required StatsSection Unranked { get; init; }

        [JsonProperty("ranked")]
        public required StatsSection Ranked { get; init; }
    }

    public readonly struct StatsSection
    {
        [JsonIgnore]
        public double WinRate =>
            Matches > 0 ? ((double) Wins / Matches) * 100 : 0;

        [JsonProperty("total_matches")]
        public required int Matches { get; init; }

        [JsonProperty("total_wins")]
        public required int Wins { get; init; }

        [JsonIgnore]
        public double Kda =>
            Kills > 0 ? (double) (Assists + Kills) / Deaths : 0;

        [JsonProperty("total_assists")]
        public required int Assists { get; init; }

        [JsonProperty("total_deaths")]
        public required int Deaths { get; init; }

        [JsonProperty("total_kills")]
        public required int Kills { get; init; }

        [JsonIgnore]
        public TimeSpan TimePlayed => TimeSpan.FromSeconds(TimePlayedSeconds);

        [JsonProperty("total_time_played_raw")]
        public required double TimePlayedSeconds { get; init; }

        [JsonProperty("total_mvp")]
        public required int MvpCount { get; init; }

        [JsonProperty("total_svp")]
        public required int SvpCount { get; init; }
    }

    public readonly struct PlayerSection
    {
        [JsonProperty("uid")]
        public required string Uid { get; init; }

        [JsonProperty("name")]
        public required string Name { get; init; }

        [JsonProperty("level")]
        public required int Level { get; init; }

        [JsonProperty("rank")]
        public required RankSection Rank { get; init; }

        [JsonProperty("team")]
        public required TeamSection Team { get; init; }

        [JsonProperty("icon")]
        public required IconSection Icon { get; init; }
    }

    public readonly struct RankSection
    {
        [JsonProperty("rank")]
        public required string Name { get; init; }

        [JsonProperty("image")]
        public required string Path { private get; init; }

        [JsonIgnore]
        public string Url => MarvelRivalsStatsCommand.BaseUrl + "/rivals" + Path;

        [JsonProperty("color")]
        public required string Color { get; init; }
    }

    public readonly struct TeamSection
    {
        [JsonProperty("club_team_mini_name")]
        public required string Name { get; init; }

        public override string ToString()
        {
            return Name;
        }
    }

    public readonly struct IconSection
    {

        [JsonProperty("player_icon")]
        public required string Path { private get; init; }

        [JsonIgnore]
        public string Url => MarvelRivalsStatsCommand.BaseUrl + "/rivals" + Path;

    }

    public readonly struct TeammateSection
    {
        [JsonProperty("player_info")]
        public required TeamMatePlayerInfoSection PlayerData { get; init; }

        [JsonProperty("matches")]
        public required int Matches { get; init; }

        [JsonProperty("win_rate")]
        public required string WinRateString { private get; init; }

        [JsonIgnore]
        public double WinRate => double.Parse(WinRateString);
    }

    public readonly struct TeamMatePlayerInfoSection
    {

        [JsonProperty("nick_name")]
        public required string Name { get; init; }

    }

}