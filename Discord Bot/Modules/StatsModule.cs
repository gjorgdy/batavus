using Discord_Bot.Utils;
using Discord;
using Discord.Interactions;
using JetBrains.Annotations;
using CoreModules.Stats.MarvelRivals.Commands;
using MrEmbedFactory = Discord_Bot.Modules.MarvelRivals.MarvelRivalsEmbedFactory;
using Pages = Discord_Bot.Modules.MarvelRivals.MarvelRivalsEmbedFactory.Pages;

namespace Discord_Bot.Modules;

[Group("stats", "Commands related to statistics of video games.")]
[UsedImplicitly] // This attribute is used to indicate that this method is used implicitly by the Discord library.
public class StatsModule(HttpClient httpClient) : InteractionModuleBase
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
        try
        {
            var player = await new PlayerCommand(httpClient, input)
                .Execute();

            await RespondAsync(
                embed: MrEmbedFactory.BuildMainStatsPage(player),
                components: MrEmbedFactory.BuildComponents(player, Pages.MainStats),
                ephemeral: ephemeral
            );
        }
        catch(Exception e)
        {
            Console.Out.WriteLine(e);
            await RespondAsync(embed: EmbedUtils.CreateErrorEmbed("Error", e.Message), ephemeral: true);
        }
    }

}