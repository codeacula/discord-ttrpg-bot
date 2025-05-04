namespace TtrpgAiBot.Discord.Extensions;

using Microsoft.Extensions.DependencyInjection;
using TtrpgAiBot.Core.Platform;
using TtrpgAiBot.Discord.Config;
using TtrpgAiBot.Discord.Infrastructure;
using TtrpgAiBot.Discord.Service;

public static class ServiceExtensions
{
  /// <summary>
  /// Adds Discord bot related services to the dependency injection container.
  /// </summary>
  /// <param name="services">The service collection to add to.</param>
  /// <returns>The updated service collection.</returns>
  public static async Task<IServiceCollection> AddDiscord(
    this IServiceCollection services,
    DiscordConfig discordConfig)
  {
    var discordGateway = new DiscordGateway(discordConfig);
    await discordGateway.StartAsync();
    
    services.AddSingleton(discordGateway);
    services.AddTransient<IPlatformIntegration, DiscordPlatform>();
    return services;
  }
}