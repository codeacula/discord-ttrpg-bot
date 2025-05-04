using TtrpgAiBot.Core.Bot;
using Microsoft.AspNetCore.Mvc;

namespace TtrpgAiBot.Api.Controllers.v1;

[ApiController]
public class TestController : BaseController
{
    [HttpGet("send-text")]
    public async Task<IActionResult> SendText([FromServices] IBot bot, [FromQuery] string text)
    {
        await bot.SayAsync(text);
        return Ok("Message sent to Discord!");
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Test endpoint is working!");
    }
}