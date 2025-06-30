using Discord;
using Discord.Interactions;
using JetBrains.Annotations;
using Logic.Roll;

namespace Discord_Bot.Modules;

[UsedImplicitly] // This attribute is used to indicate that this method is used implicitly by the Discord library.
public class RollModule : InteractionModuleBase
{

    [CommandContextType(InteractionContextType.Guild, InteractionContextType.BotDm, InteractionContextType.PrivateChannel)]
    [IntegrationType(ApplicationIntegrationType.UserInstall, ApplicationIntegrationType.GuildInstall)]
    [SlashCommand("roll", "Rolls a dice.")]
    [UsedImplicitly] // This attribute is used to indicate that this method is used implicitly by the Discord library.
    public async Task Roll([Summary("input", "The dice to roll, example; 2d4")] string input)
    {
        var response = await new RollCommand(input)
            .Execute();

        var embed = new EmbedBuilder()
            .WithTitle("Rolling " + input)
            .WithDescription($"> {string.Join(" + ", response.Components)}\n**= {response.Total}**")
            .WithColor(Color.Blue)
            .Build();

        await RespondAsync(embed: embed);
    }

}