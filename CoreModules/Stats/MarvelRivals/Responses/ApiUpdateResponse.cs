using Newtonsoft.Json;

namespace CoreModules.Stats.MarvelRivals.Responses;

public class ApiUpdateResponse
{
    [JsonProperty("success")]
    public required bool Success { get; set; }

    [JsonProperty("message")]
    public required string Message { get; set; }
}