using Microsoft.AspNetCore.Mvc;

namespace DiscordTtrpgBot.Api.Controllers.v1;

[ApiController]
public class TestController : BaseController
{
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Test endpoint is working!");
    }
}