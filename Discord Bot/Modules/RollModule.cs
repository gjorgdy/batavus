using Core.Services;
using Discord;
using Discord.Interactions;
using Logic.Roll;

namespace Discord_Bot.Modules;

public class RollModule : InteractionModuleBase
{

    [CommandContextType(InteractionContextType.Guild, InteractionContextType.BotDm, InteractionContextType.PrivateChannel)]
    [IntegrationType(ApplicationIntegrationType.UserInstall, ApplicationIntegrationType.GuildInstall)]
    [SlashCommand("roll", "Rolls a dice.")]
    public async Task Roll([Summary("input", "The dice to roll, example; 2d4")] string input)
    {
        var response = new RollCommand(input)
            .Execute();

        var embed = new EmbedBuilder()
            .WithTitle("Rolling " + input)
            .WithDescription($"> {string.Join(" ", response.Result.Components)}\n**= {response.Result.Total}**")
            .WithColor(Color.Blue)
            .Build();

        await RespondAsync(embed: embed);
    }

}