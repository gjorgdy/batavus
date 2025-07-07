using Newtonsoft.Json;

namespace CoreModules.Stats.MarvelRivals.Models;

public class ApiUpdateResponse
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}