using NetCord.Services.ApplicationCommands;

namespace TtrpgAiBot.Discord.Modules;

public class TestModule : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("test", "Send a test message")]
    public static string Test() => "Test message";
}
