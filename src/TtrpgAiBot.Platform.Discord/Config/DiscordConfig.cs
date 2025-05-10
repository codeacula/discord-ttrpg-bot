namespace TtrpgAiBot.Platform.Discord.Config;

/// <summary>
/// Represents the configuration settings required for integrating with Discord.
/// </summary>
public record DiscordConfig
{
  /// <summary>
  /// Gets the client ID of the Discord application.
  /// </summary>
  public required string ClientId { get; init; }

  /// <summary>
  /// Gets the client secret of the Discord application.
  /// </summary>
  public required string ClientSecret { get; init; }

  /// <summary>
  /// Gets the bot token used for authenticating the Discord bot.
  /// </summary>
  public required string BotToken { get; init; }
}
