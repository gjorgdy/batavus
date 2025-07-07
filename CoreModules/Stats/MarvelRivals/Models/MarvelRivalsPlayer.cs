using System.Net.Http.Json;
using Core.Config;
using CoreModules.Stats.MarvelRivals.Responses;
using Newtonsoft.Json;

namespace CoreModules.Stats.MarvelRivals.Models;

public class MarvelRivalsPlayer : ApiPlayerResponse
{

    public async Task<bool> Update(HttpClient httpClient)
    {
        httpClient.DefaultRequestHeaders.Add("x-api-key", EnvironmentVars.MarvelRivalsApiKey);
        var response = await httpClient.GetAsync(
            new Uri($"{MarvelRivalsStatsCommand.BaseUrl}/api/v1/player/{Data.Uid}/update")
        );

        if (response.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<ApiUpdateResponse>(await response.Content.ReadAsStringAsync())?.Success ?? false;
        }

        var errorResponse = await response.Content.ReadFromJsonAsync<ApiExceptionResponse>();
        throw new Exception($"Error: {errorResponse.Message}, Status: {response.StatusCode}");
    }

}