namespace TtrpgAiBot.Api.Controllers.V1;

/// <summary>
/// Controller for testing API endpoints.
/// </summary>
/// <param name="bot">The bot instance used for sending messages.</param>
[ApiController]
public class TestController(IBot bot) : BaseController
{
  /// <summary>
  /// Sends a text message to Discord.
  /// </summary>
  /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
  [HttpGet("send-text")]
  public async Task<IActionResult> SendTextAsync()
  {
    await bot.SayAsync("Butts");
    return Ok("Message sent to Discord!");
  }
}
