using Microsoft.Extensions.DependencyInjection;
using NetCord.Hosting.Gateway;

namespace DiscordTtrpgBot.Discord;

public static class ServiceExtensions
{
  /// <summary>
  /// Adds Discord bot related services to the dependency injection container.
  /// </summary>
  /// <param name="services">The service collection to add to.</param>
  /// <returns>The updated service collection.</returns>
  public static IServiceCollection AddDiscordTtrpgBotDiscord(this IServiceCollection services)
  {
    // Register your Discord bot services here.
    // Example:
    // services.AddSingleton<IDiscordBotService, DiscordBotService>();
    services.AddDiscordGateway();

    return services;
  }
}