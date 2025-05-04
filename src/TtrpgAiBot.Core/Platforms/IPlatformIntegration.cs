namespace TtrpgAiBot.Core.Platform;

public interface IPlatformIntegration
{
    /// <summary>
    /// Sends a message to the platform.
    /// </summary>
    /// <param name="message">The message to send.</param>
    Task SendMessageAsync(string message);
}