using CoreModules.Stats.MarvelRivals;
using Discord_Bot.Modules.MarvelRivals;
using Discord_Bot.Utils;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using JetBrains.Annotations;
using MrEmbedFactory = Discord_Bot.Modules.MarvelRivals.MarvelRivalsEmbedFactory;
using Pages = Discord_Bot.Modules.MarvelRivals.MarvelRivalsEmbedFactory.Pages;

namespace Discord_Bot.Modules;

// [Group("stats", "Commands related to statistics of video games.")]
[UsedImplicitly] // This attribute is used to indicate that this method is used implicitly by the Discord library.
public class StatsModule(MarvelRivalsPlayerService playerService) : InteractionModuleBase
{
    [CommandContextType(InteractionContextType.Guild, InteractionContextType.BotDm, InteractionContextType.PrivateChannel)]
    [IntegrationType(ApplicationIntegrationType.UserInstall, ApplicationIntegrationType.GuildInstall)]
    [SlashCommand("rivals", "Get the stats of a player in Marvel Rivals.")]
    [UsedImplicitly] // This attribute is used to indicate that this method is used implicitly by the Discord library.
    public async Task StatsRivals(
        [Summary("player", "The username or uid of the player")] string input,
        [Summary("hidden", "If only you should be able to see this command")] bool ephemeral = false
    )
    {
        await DeferAsync(ephemeral: ephemeral);
        try
        {
            var player = await playerService.GetUser(input);
            await ModifyOriginalResponseAsync(msg =>
                {
                    msg.Embed = MrEmbedFactory.BuildGeneralStatsPage(player);
                    msg.Components = MrEmbedFactory.BuildComponents(player, Pages.GeneralStats);
                }
            );
        }
        catch(Exception e)
        {
            await ModifyOriginalResponseAsync(msg =>
                {
                    msg.Embed = EmbedUtils.CreateErrorEmbed("Error", e.Message);
                }
            );
        }
    }

    public async Task ButtonHandler(SocketMessageComponent component)
    {
        string customId = component.Data.CustomId;
        string[] parts = customId.Split('_');
        if (parts[0] != "smr") return;
        var player = await playerService.GetUser(parts[2]);
        switch (parts[1])
        {
            case "teammates":
            {
                await component.UpdateAsync(msg =>
                {
                    msg.Embed = MrEmbedFactory.BuildTeammatesPage(player);
                    msg.Components = MrEmbedFactory.BuildComponents(player, Pages.Teammates);
                });
                break;
            }
            case "stats":
            {
                await component.UpdateAsync(msg =>
                {
                    msg.Embed = MrEmbedFactory.BuildGeneralStatsPage(player);
                    msg.Components = MrEmbedFactory.BuildComponents(player, Pages.GeneralStats);
                });
                break;
            }
            case "ranked":
            {
                await component.UpdateAsync(msg =>
                {
                    msg.Embed = MrEmbedFactory.BuildRankedStatsPage(player);
                    msg.Components = MrEmbedFactory.BuildComponents(player, Pages.Ranked);
                });
                break;
            }
            case "unranked":
            {
                await component.UpdateAsync(msg =>
                {
                    msg.Embed = MrEmbedFactory.BuildUnrankedStatsPage(player);
                    msg.Components = MrEmbedFactory.BuildComponents(player, Pages.Unranked);
                });
                break;
            }
        }
    }

}