namespace TtrpgAiBot.Discord.Extensions;

using Microsoft.Extensions.DependencyInjection;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;
using TtrpgAiBot.Core.Platform;
using TtrpgAiBot.Discord.Config;
using TtrpgAiBot.Discord.Infrastructure;
using TtrpgAiBot.Discord.Service;

public static class ServiceExtensions
{
  /// <summary>
  /// Adds Discord bot related services to the dependency injection container.
  /// </summary>
  /// <param name="services">The service collection to add to.</param>
  /// <returns>The updated service collection.</returns>
  public static async Task<IServiceCollection> AddDiscord(
    this IServiceCollection services,
    DiscordConfig discordConfig)
  {
    ApplicationCommandService<ApplicationCommandContext> applicationCommandService = new();
    applicationCommandService.AddModules(typeof(ServiceExtensions).Assembly);

    var discordGateway = new DiscordGateway(discordConfig);
    await discordGateway.StartAsync();

    services.AddApplicationCommands();

    await applicationCommandService.CreateCommandsAsync(discordGateway.GatewayClient.Rest, discordGateway.GatewayClient.Id);

    discordGateway.GatewayClient.Log += message =>
    {
        Console.WriteLine(message);
        return default;
    };

    discordGateway.GatewayClient.InteractionCreate += async interaction =>
    {
        // Check if the interaction is an application command interaction
        if (interaction is not NetCord.ApplicationCommandInteraction applicationCommandInteraction)
            return;

        // Execute the command
        var result = await applicationCommandService.ExecuteAsync(new ApplicationCommandContext(applicationCommandInteraction, discordGateway.GatewayClient));

        // Check if the execution failed
        if (result is not IFailResult failResult)
            return;

        // Return the error message to the user if the execution failed
        try
        {
            await interaction.SendResponseAsync(InteractionCallback.Message(failResult.Message));
        }
        catch
        {
        }
    };

    services.AddSingleton(discordGateway);
    services.AddSingleton(discordGateway.GatewayClient.Rest);
    services.AddTransient<IPlatformIntegration, DiscordPlatform>();
    return services;
  }
}