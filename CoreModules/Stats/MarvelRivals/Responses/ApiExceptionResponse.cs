using System.Text.Json.Serialization;

namespace CoreModules.Stats.MarvelRivals.Responses;

public struct ApiExceptionResponse
{

    [JsonPropertyName("error")]
    public required bool Error { get; init; }

    [JsonPropertyName("message")]
    public required string Message { get; init; }

    [JsonPropertyName("status")]
    public required int Status { get; init; }

}