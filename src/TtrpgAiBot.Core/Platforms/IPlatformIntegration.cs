namespace TtrpgAiBot.Core.Platforms;

public interface IPlatformIntegration
{
  /// <summary>
  /// Sends a message to the platform.
  /// </summary>
  /// <param name="message">The message to send.</param>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  Task SendMessageAsync(string message);
}
