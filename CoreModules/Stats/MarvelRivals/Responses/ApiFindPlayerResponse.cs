using System.Text.Json.Serialization;

namespace CoreModules.Stats.MarvelRivals.Models;

public class ApiFindPlayerResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("uid")]
    public string Uid { get; set; }
}