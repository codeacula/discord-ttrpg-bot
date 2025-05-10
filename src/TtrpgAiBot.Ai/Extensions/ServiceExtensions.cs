namespace TtrpgAiBot.Ai.Extensions;

using Microsoft.Extensions.DependencyInjection;
using TtrpgAiBot.Ai.Config;
using TtrpgAiBot.Ai.Services;
using TtrpgAiBot.Core.AI.Services;

/// <summary>
/// Adds Discord bot related services to the dependency injection container.
/// </summary>
public static class ServiceExtensions
{
  /// <summary>
  /// Adds Discord bot related services to the dependency injection container.
  /// </summary>
  /// <param name="services">The service collection to add to.</param>
  /// <param name="configuration">The configuration instance.</param>
  /// <returns>The updated service collection.</returns>
  public static IServiceCollection AddAiServices(
    this IServiceCollection services,
    AIConfig configuration)
  {
    // Register the OpenAICompletionService
    services.AddScoped<IOpenAICompletionService, OpenAICompletionService>();

    return services;
  }
}
