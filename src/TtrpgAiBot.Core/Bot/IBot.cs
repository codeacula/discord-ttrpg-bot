namespace TtrpgAiBot.Core.Bot;

public interface IBot
{
  Task SayAsync(string message, CancellationToken cancellationToken = default);
}
