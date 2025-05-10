namespace TtrpgAiBot.Platform.Discord.Services;

using Microsoft.Extensions.Logging;
using TtrpgAiBot.Core.Platform;
using TtrpgAiBot.Platform.Discord.Config;
using TtrpgAiBot.Platform.Discord.Infrastructure;

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