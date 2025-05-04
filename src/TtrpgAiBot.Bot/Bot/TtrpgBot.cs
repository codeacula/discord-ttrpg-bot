namespace TtrpgAiBot.Bot;

using TtrpgAiBot.Core.Bot;
using TtrpgAiBot.Core.Platform;

public sealed class TtrpgBot(IPlatformIntegration platformIntegration) : IBot
{
    private readonly IPlatformIntegration _platformIntegration = platformIntegration;

  public async Task SayAsync(string message)
    {
        await _platformIntegration.SendMessageAsync(message);
    }
}