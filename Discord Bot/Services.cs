using Core.Services;
using CoreModules.Stats.MarvelRivals;
using Discord_Bot.Modules;
using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Discord_Bot;

public static class Services
{
    public static ServiceProvider Provider => _serviceProvider ??= CreateProvider();

    private static ServiceProvider? _serviceProvider;

    /// <summary>
    /// Registers the services required for the application.
    /// </summary>
    /// <returns>A <see cref="ServiceProvider"/> instance with registered services.</returns>
    private static ServiceProvider CreateProvider()
    {
        var collection = new ServiceCollection()
            .AddSingleton<LoggerService>()
            .AddSingleton<Bot>()
            .AddSingleton<InteractionHandler>()
            .AddSingleton(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.Guilds,
            })
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<IRestClientProvider, DiscordSocketClient>()
            // Scoped services
            .AddTransient<HttpClient>()
            // Transient services, modules
            .AddTransient<MarvelRivalsPlayerService>()
            // Discord modules
            .AddTransient<StatsModule>();
        return collection.BuildServiceProvider();
    }

    /// <summary>
    /// Retrieves a required service of type <typeparamref name="T"/> from the service provider.
    /// </summary>
    /// <typeparam name="T">The type of the service to retrieve. Must not be null.</typeparam>
    /// <returns>An instance of the requested service type.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the requested service is not registered.</exception>
    public static T Get<T>() where T : notnull
    {
        return Provider.GetRequiredService<T>();
    }

    /// <summary>
    /// Retrieves a service of the specified <see cref="Type"/> from the service provider.
    /// </summary>
    /// <param name="t">The type of the service to retrieve.</param>
    /// <returns>An instance of the requested service type, or null if not found.</returns>
    public static object? Get(Type t)
    {
        return Provider.GetService(t);
    }
}