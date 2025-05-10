namespace TtrpgAiBot.Platform.Discord.Infrastructure;

using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using TtrpgAiBot.Core.Platforms;
using TtrpgAiBot.Platform.Discord.Config;

/// <summary>
/// Represents the gateway for interacting with Discord's API.
/// This class provides methods to send messages and start the gateway client.
/// </summary>
/// <param name="discordConfig">The configuration settings for the Discord platform.</param>
public class DiscordGateway(DiscordConfig discordConfig) : IPlatformIntegration
{
  /// <summary>
  /// The client used to interact with Discord's gateway API.
  /// Provides functionality for real-time communication with Discord.
  /// </summary>
#pragma warning disable SA1401 // Fields should be private
  public readonly GatewayClient GatewayClient = new(new BotToken(discordConfig.BotToken), new GatewayClientConfiguration()
#pragma warning restore SA1401 // Fields should be private
  {
    Intents = default,
  });

  /// <summary>
  /// Sends a message to a Discord text channel asynchronously.
  /// </summary>
  /// <param name="text">The content of the message to be sent.</param>
  /// <returns>A task that represents the asynchronous operation.</returns>
  public async Task SendMessageAsync(string text)
  {
    MessageProperties message = new();

    message
      .WithContent(text)
      .WithComponents([]);

    // Get the channel as a generic Channel
    var channel = await GatewayClient.Rest.GetChannelAsync(1317933225108045834);
    if (channel is TextChannel textChannel)
    {
      await textChannel.SendMessageAsync(message);
    }
    else
    {
      // Optionally log or handle the case where the channel is not a text channel
      // Example: Console.WriteLine("Channel is not a text channel.");
    }
  }

  /// <summary>
  /// Starts the Discord gateway client asynchronously.
  /// </summary>
  /// <returns>A task that represents the asynchronous operation.</returns>
  public async Task StartAsync()
  {
    await GatewayClient.StartAsync();
  }
}
