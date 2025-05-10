namespace TtrpgAiBot.Ai.Config;

/// <summary>
/// Represents the configuration settings for the AI system.
/// </summary>
public record AIConfig
{
  /// <summary>
  /// Gets the base URL of the AI service.
  /// </summary>
  public required string BaseUrl { get; init; }

  /// <summary>
  /// Gets the API key used to authenticate with the AI service.
  /// </summary>
  public required string ApiKey { get; init; }

  /// <summary>
  /// Gets the name of the AI model to be used.
  /// </summary>
  public required string Model { get; init; }

  /// <summary>
  /// Gets the maximum number of tokens allowed in the AI response.
  /// </summary>
  public required int MaxTokens { get; init; }
}
