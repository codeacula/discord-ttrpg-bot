namespace TtrpgAiBot.Bot;

using Microsoft.Extensions.DependencyInjection;
using TtrpgAiBot.Bot.Bot;
using TtrpgAiBot.Core.Bot;

public static class ServiceExtensions
{
    /// <summary>
    /// Adds Discord bot related services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddBot(this IServiceCollection services)
    {
        services.AddTransient<IBot, TtrpgBot>();
        return services;
    }
}