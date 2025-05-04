namespace TtrpgAiBot.Discord.Service;

using Microsoft.Extensions.Logging;
using TtrpgAiBot.Core.Platform;
using TtrpgAiBot.Discord.Config;
using TtrpgAiBot.Discord.Infrastructure;

public class DiscordPlatform(DiscordConfig discordConfig, ILogger<DiscordPlatform> logger, DiscordGateway discordGateway) : IPlatformIntegration
{
  private readonly DiscordConfig _discordConfig = discordConfig;
  private readonly ILogger<DiscordPlatform> _logger = logger;
  private readonly DiscordGateway _discordGateway = discordGateway;

  public async Task SendMessageAsync(string message)
  {

    await _discordGateway.SendMessageAsync(message);
  }
}