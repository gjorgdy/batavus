using CoreModules.Stats.MarvelRivals.Commands;
using Discord.WebSocket;

namespace Discord_Bot.Modules.MarvelRivals;

public class MarvelRivalsButtonHandler(HttpClient httpClient)
{
    public async Task ButtonHandler(SocketMessageComponent component)
    {
        string customId = component.Data.CustomId;
        string[] parts = customId.Split('_');
        if (parts[0] != "smr") return;
        if (parts[1] == "teammates")
        {
            var player = await new PlayerCommand(httpClient, parts[2])
                .Execute();
            await component.UpdateAsync(msg =>
            {
                msg.Embed = MarvelRivalsEmbedFactory.BuildTeammatesPage(player);
                msg.Components = MarvelRivalsEmbedFactory.BuildComponents(player, MarvelRivalsEmbedFactory.Pages.Teammates);
            });
        }
    }
}