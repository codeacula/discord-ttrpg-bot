namespace TtrpgAiBot.Discord.Infrastructure;

using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using TtrpgAiBot.Core.Platform;
using TtrpgAiBot.Discord.Config;

public class DiscordGateway(DiscordConfig discordConfig) : IPlatformIntegration
{
  private readonly GatewayClient _gatewayClient = new(new BotToken(discordConfig.ClientSecret));

  public async Task SendMessageAsync(string text)
  {

    MessageProperties message = new();

    message
      .WithContent(text)
      .WithComponents([]);

    // Get the channel as a generic Channel
    var channel = await _gatewayClient.Rest.GetChannelAsync(1317933225108045834);
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

  public async Task StartAsync()
  {
    await _gatewayClient.StartAsync();
  }
}