using System.Collections;
using System.Reflection;
using Core.Config;
using Core.Services;
using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;

namespace Discord_Bot;

public class InteractionHandler(DiscordSocketClient client, LoggerService logger)
{

    private InteractionService? _service;

    public async Task OnInteraction(SocketInteraction interaction)
    {
        try
        {
            SocketInteractionContext ctx = new(client, interaction);
            if (_service == null) return;
            await _service.ExecuteCommandAsync(ctx, Services.Provider);
        }
        catch
        {
            if (interaction.Type == InteractionType.ApplicationCommand)
            {
                await interaction
                    .GetOriginalResponseAsync()
                    .ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }
    }

    public async Task RegisterInteractions()
    {
        _service = new InteractionService(
            client, new InteractionServiceConfig
            {
                UseCompiledLambda = true,
                ThrowOnError = true
            }
        );

        _service.SlashCommandExecuted += (sci, ctx, _) =>
        {
            if (!ctx.Interaction.HasResponded)
            {
                ctx.Interaction.RespondAsync(embed: new EmbedBuilder()
                    .WithTitle("Error")
                    .WithDescription($"Command `{sci.Name}` failed.")
                    .WithColor(Color.DarkRed)
                    .Build()
                );
            }
            return Task.CompletedTask;
        };

        await _service.AddModulesAsync(
            Assembly.GetExecutingAssembly(),
            Services.Provider
        );

        ulong? guildId = EnvironmentVars.DiscordDevGuildId;
        IEnumerable commands;
        if (guildId != null)
        {
            commands = await _service.RegisterCommandsToGuildAsync(guildId.Value);
            string guildName = client.GetGuild(guildId.Value).Name;
            logger.Info(this, "Registering guild commands for " + guildName);
        }
        else
        {
            commands = await _service.RegisterCommandsGloballyAsync();
            logger.Info(this, "Registering global commands");
        }

        foreach (object command in commands)
        {
            if (command is RestApplicationCommand restCommand)
            {
                logger.Verbose(this, $"Registered command: {restCommand.Name} ({restCommand.Id})");
            }
        }
    }

}