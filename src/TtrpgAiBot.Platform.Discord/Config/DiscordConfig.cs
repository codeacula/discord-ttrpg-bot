namespace TtrpgAiBot.Platform.Discord.Config;

public record DiscordConfig
{
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
    public required string BotToken { get; init; }
}