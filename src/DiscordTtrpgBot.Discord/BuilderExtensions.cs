using Microsoft.Extensions.Hosting;
using NetCord.Hosting.Gateway;

namespace DiscordTtrpgBot.Discord;

public static class Build
{
  /// <summary>
  /// Adds Discord bot related services to the dependency injection container.
  /// </summary>
  /// <param name="services">The service collection to add to.</param>
  /// <returns>The updated service collection.</returns>
  public static IHostBuilder AddDiscordTtrpgBotDiscord(this IHostBuilder hostBuilder)
  {
    // Register your Discord bot services here.
    // Example:
    // services.AddSingleton<IDiscordBotService, DiscordBotService>();

    hostBuilder.UseDiscordGateway();

    return hostBuilder;
  }
}