using System.Net.Http.Json;
using Core.Config;
using Core.Services;
using CoreModules.Stats.MarvelRivals.Responses;
using Newtonsoft.Json;

namespace CoreModules.Stats.MarvelRivals.Models;

public class MarvelRivalsPlayer : ApiPlayerResponse
{

    public async Task<bool> Update(HttpClient httpClient, LoggerService logger)
    {
        httpClient.DefaultRequestHeaders.Add("x-api-key", EnvironmentVars.MarvelRivalsApiKey);
        httpClient.Timeout = TimeSpan.FromSeconds(30); // Set a timeout for the request
        var response = await httpClient.GetAsync(
            new Uri($"{MarvelRivalsPlayerService.BaseUrl}/api/v1/player/{Data.Uid}/update")
        );

        // if (response.IsSuccessStatusCode)
        // {
            var updateResponse = JsonConvert.DeserializeObject<ApiUpdateResponse>(await response.Content.ReadAsStringAsync());

            logger.Verbose(this,$"{(updateResponse?.Success ?? false ? "Success" : "Failure")} - {updateResponse?.Message}");

            return updateResponse?.Success ?? false;;
        // }

        var errorResponse = await response.Content.ReadFromJsonAsync<ApiExceptionResponse>();
        logger.Error(this, $"Error trying to update Marvel Rivals Player: {errorResponse.Message}");
    }

}