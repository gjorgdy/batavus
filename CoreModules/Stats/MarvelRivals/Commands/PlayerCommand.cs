using System.Net.Http.Json;
using Core.Config;
using Core.Interfaces;
using CoreModules.Stats.MarvelRivals.Models;
using Newtonsoft.Json;

namespace CoreModules.Stats.MarvelRivals.Commands;

public class PlayerCommand(HttpClient httpClient, string player) : ICommand<MarvelRivalsPlayer>
{

    public async Task<MarvelRivalsPlayer> Execute()
    {
        httpClient.DefaultRequestHeaders.Add("x-api-key", EnvironmentVars.MarvelRivalsApiKey);
        var response = await httpClient.GetAsync(
            new Uri($"{MarvelRivalsStatsCommand.BaseUrl}/api/v1/player/{player}")
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
            if (player.All(char.IsDigit))
            {
                throw new ArgumentException($"Player with uid '{player}' not found.");
            }
            throw new ArgumentException($"Player '{player}' not found.");
        }

        var errorResponse = await response.Content.ReadFromJsonAsync<ApiExceptionResponse>();
        throw new Exception($"Error: {errorResponse.Message}, Status: {response.StatusCode}");
    }

}