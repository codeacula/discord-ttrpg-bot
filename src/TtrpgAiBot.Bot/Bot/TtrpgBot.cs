namespace TtrpgAiBot.Bot.Bot;

using TtrpgAiBot.Core.Bot;
using TtrpgAiBot.Core.Platforms;

/// <summary>
/// Represents a bot for tabletop role-playing games (TTRPGs) that integrates with a platform
/// to send messages and interact with users.
/// </summary>
/// <param name="platformIntegration">
/// The platform integration instance used to send messages and interact with the platform.
/// </param>
public sealed class TtrpgBot(IPlatformIntegration platformIntegration) : IBot
{
  private readonly IPlatformIntegration platformIntegration = platformIntegration;

  /// <summary>
  /// Sends a message asynchronously using the platform integration.
  /// </summary>
  /// <param name="message">The message to be sent.</param>
  /// <returns>A task that represents the asynchronous operation.</returns>
  public async Task SayAsync(string message)
  {
    await platformIntegration.SendMessageAsync(message);
  }
}
