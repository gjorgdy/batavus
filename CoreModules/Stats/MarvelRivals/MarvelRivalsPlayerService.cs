using System.Net.Http.Json;
using Core.Config;
using Core.Interfaces;
using CoreModules.Stats.MarvelRivals.Models;
using Newtonsoft.Json;

namespace CoreModules.Stats.MarvelRivals;

public class MarvelRivalsPlayerService(HttpClient httpClient)
{

    public async Task<MarvelRivalsPlayer> GetUser(string playerIdentifier)
    {
        httpClient.DefaultRequestHeaders.Add("x-api-key", EnvironmentVars.MarvelRivalsApiKey);
        var response = await httpClient.GetAsync(
            new Uri($"{MarvelRivalsStatsCommand.BaseUrl}/api/v1/player/{playerIdentifier}")
        );

        if (response.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<MarvelRivalsPlayer>(await response.Content.ReadAsStringAsync())
                ?? throw new ArgumentException("Failed to deserialize player response.");
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