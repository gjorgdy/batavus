using System.Text.Json.Serialization;

namespace CoreModules.Stats.MarvelRivals.Models;

public struct ApiExceptionResponse
{

    [JsonPropertyName("error")]
    public bool Error { get; }

    [JsonPropertyName("message")]
    public string Message { get; }

    [JsonPropertyName("status")]
    public int Status { get; }

}