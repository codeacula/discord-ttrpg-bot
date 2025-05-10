namespace TtrpgAiBot.Platform.Discord.Services;

using Microsoft.Extensions.Logging;
using TtrpgAiBot.Core.Platforms;
using TtrpgAiBot.Platform.Discord.Config;
using TtrpgAiBot.Platform.Discord.Infrastructure;

/// <summary>
/// Represents the Discord platform integration for the TTRPG AI Bot.
/// This class handles sending messages to Discord using the provided configuration,
/// logger, and gateway.
/// </summary>
/// <param name="discordConfig">The configuration settings for the Discord platform.</param>
/// <param name="logger">The logger instance for logging platform-related activities.</param>
/// <param name="discordGateway">The gateway for interacting with Discord's API.</param>
public class DiscordPlatform(DiscordConfig discordConfig, ILogger<DiscordPlatform> logger, DiscordGateway discordGateway) : IPlatformIntegration
{
  private readonly DiscordConfig discordConfig = discordConfig;
  private readonly ILogger<DiscordPlatform> logger = logger;
  private readonly DiscordGateway discordGateway = discordGateway;

  /// <summary>
  /// Sends a message to the Discord platform asynchronously.
  /// </summary>
  /// <param name="message">The message content to be sent to Discord.</param>
  /// <returns>A task that represents the asynchronous operation.</returns>
  public async Task SendMessageAsync(string message)
  {
    await discordGateway.SendMessageAsync(message);
  }
}
