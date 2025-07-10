using System.Net.Http.Json;
using Core.Config;
using Core.Interfaces;
using Core.Services;
using CoreModules.Stats.MarvelRivals.Models;
using CoreModules.Stats.MarvelRivals.Responses;
using Newtonsoft.Json;

namespace CoreModules.Stats.MarvelRivals;

public class MarvelRivalsPlayerService(HttpClient httpClient, LoggerService logger)
{
    public const string BaseUrl = "https://marvelrivalsapi.com";

    public async Task<MarvelRivalsPlayer> GetUser(string playerIdentifier)
    {
        httpClient.DefaultRequestHeaders.Add("x-api-key", EnvironmentVars.MarvelRivalsApiKey);
        var response = await httpClient.GetAsync(
            new Uri($"{BaseUrl}/api/v1/player/{playerIdentifier}")
        );

        if (response.IsSuccessStatusCode)
        {
            var player = JsonConvert.DeserializeObject<MarvelRivalsPlayer>(await response.Content.ReadAsStringAsync())
                ?? throw new ArgumentException("Failed to deserialize player response.");
            // Update the player if the last update request was more than 30 minutes ago
            if (player.Updates.Last == null || player.Updates.Last.Value.AddMinutes(30) < DateTime.UtcNow)
            {
                _ = player.Update(httpClient, logger);
            }
            return player;
        }

        if (response.StatusCode
            is System.Net.HttpStatusCode.NotFound
            or System.Net.HttpStatusCode.InternalServerError
            or System.Net.HttpStatusCode.BadRequest
        )
        {
            if (playerIdentifier.All(char.IsDigit))
            {
                throw new ArgumentException($"Player with uid '{playerIdentifier}' not found.");
            }
            throw new ArgumentException($"Player '{playerIdentifier}' not found.");
        }

        var errorResponse = await response.Content.ReadFromJsonAsync<ApiExceptionResponse>();
        throw new Exception($"Error: {errorResponse.Message}, Status: {response.StatusCode}");
    }

}