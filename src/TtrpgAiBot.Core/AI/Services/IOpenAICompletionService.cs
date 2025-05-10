namespace TtrpgAiBot.Core.AI.Services;

public interface IOpenAICompletionService
{
  /// <summary>
  /// Asynchronously generates a completion using the OpenAI API.
  /// </summary>
  /// <param name="prompt">The prompt to generate a completion for.</param>
  /// <param name="maxTokens">The maximum number of tokens to generate.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the generated completion.</returns>
  Task<string> GenerateCompletionAsync(string prompt, int maxTokens);
}
