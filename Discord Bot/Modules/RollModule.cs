using Discord_Bot.Utils;
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
        Embed response;
        try
        {
            var result = await new RollCommand(input)
                .Execute();

            response = EmbedUtils.CreateBasicEmbed(
                "Rolling " + input,
                $"*{string.Join(" ", result.ResultStrings)}*\n> {string.Join(" ", result.CalculationStrings)}\n**= {result.Total}**",
                Color.Blue
            );
        }
        catch(Exception e)
        {
            Console.Out.WriteLine(e);
            response = EmbedUtils.CreateErrorEmbed("Error", e.Message);
        }

        await RespondAsync(embed: response);
    }

}