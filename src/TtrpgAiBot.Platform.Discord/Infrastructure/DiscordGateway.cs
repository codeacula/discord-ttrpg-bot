namespace TtrpgAiBot.Platform.Discord.Infrastructure;

using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using TtrpgAiBot.Core.Platform;
using TtrpgAiBot.Platform.Discord.Config;

public class DiscordGateway(DiscordConfig discordConfig) : IPlatformIntegration
{
    public readonly GatewayClient GatewayClient = new(new BotToken(discordConfig.BotToken), new GatewayClientConfiguration()
    {
        Intents = default,
    });

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

    public async Task StartAsync()
    {
        await GatewayClient.StartAsync();
    }
}