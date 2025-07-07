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