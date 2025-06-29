using Core.Interfaces;
using Core.Services;
using Discord_Bot;
using Microsoft.Extensions.DependencyInjection;

namespace CommandLine;

public static class Services
{
    private static ServiceProvider Provider => _serviceProvider ??= CreateProvider();

    private static ServiceProvider? _serviceProvider;

    /// <summary>
    /// Registers the services required for the application.
    /// </summary>
    /// <returns>A <see cref="ServiceProvider"/> instance with registered services.</returns>
    private static ServiceProvider CreateProvider()
    {
        var collection = new ServiceCollection()
            .AddSingleton<LoggerService>()
            .AddSingleton<IDiscordBot, DiscordBot>();
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