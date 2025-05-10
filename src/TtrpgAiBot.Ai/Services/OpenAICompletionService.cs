namespace TtrpgAiBot.Ai.Services;

using TtrpgAiBot.Core.AI.Services;

/// <summary>
/// Service for generating completions using the OpenAI API.
/// Implements the <see cref="IOpenAICompletionService"/> interface.
/// </summary>
public class OpenAICompletionService : IOpenAICompletionService
{
  /// <summary>
  /// Asynchronously generates a completion using the OpenAI API.
  /// </summary>
  /// <param name="prompt">The prompt to generate a completion for.</param>
  /// <param name="maxTokens">The maximum number of tokens to generate.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the generated completion.</returns>
  public Task<string> GenerateCompletionAsync(string prompt, int maxTokens)
  {
    throw new NotImplementedException();
  }
}
