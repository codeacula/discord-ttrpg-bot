namespace TtrpgAiBot.Api.Controllers.V1;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Serves as the base class for all API controllers in the application.
/// Provides common functionality and configuration for derived controllers.
/// </summary>
/// <remarks>
/// This controller is marked as an <see cref="ApiController"/> and uses a route prefix of "v1/[controller]".
/// Derived controllers will inherit these attributes and can define additional routes or actions.
/// </remarks>
[ApiController]
public abstract class BaseController : ControllerBase
{
}
