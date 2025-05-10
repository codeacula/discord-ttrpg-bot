namespace TtrpgAiBot.Platform.Discord.Modules;

using NetCord.Services.ApplicationCommands;

/// <summary>
/// Represents a module for handling test commands in the Discord platform.
/// </summary>
public class TestModule : ApplicationCommandModule<ApplicationCommandContext>
{
  /// <summary>
  /// A slash command that sends a test message.
  /// </summary>
  /// <returns>A string containing the test message.</returns>
  [SlashCommand("test", "Send a test message")]
  public static string Test() => "Test message";
}
