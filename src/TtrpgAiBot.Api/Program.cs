using NetCord.Hosting.Services;
using TtrpgAiBot.Discord;
using TtrpgAiBot.Discord.Config;
using TtrpgAiBot.Discord.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure logging from configuration (appsettings.json, etc.)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

// builder.Configuration.
builder.Configuration.AddUserSecrets<Program>()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

// Add versioned API explorer for Swagger
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Add Swagger generation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configs
var discordConfigSection = builder.Configuration.GetSection("TtrpgAiBot:Platforms:Discord");
builder.Services.Configure<DiscordConfig>(discordConfigSection);

// Safely bind DiscordConfig and handle missing config
var discordConfig = discordConfigSection.Get<DiscordConfig>() ?? throw new InvalidOperationException("DiscordConfig section is missing or invalid in configuration.");

// Add domain services
await builder.Services.AddDiscord(discordConfig);
builder.Services.AddSingleton(discordConfig);
builder.Services.AddBot();

var app = builder.Build();

// Get logger instance
var logger = app.Services.GetRequiredService<ILogger<Program>>();

app.AddModules(typeof(Program).Assembly);

try
{
    logger.LogInformation("Starting TTRPG AI Bot API...");
    // Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
    logger.LogInformation("TTRPG AI Bot API stopped gracefully.");
}
catch (Exception ex)
{
    logger.LogCritical(ex, "TTRPG AI Bot API terminated unexpectedly.");
    throw;
}
