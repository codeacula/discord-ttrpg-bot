using NetCord.Hosting.Services;
using TtrpgAiBot.Ai.Config;
using TtrpgAiBot.Ai.Extensions;
using TtrpgAiBot.Bot;
using TtrpgAiBot.Platform.Discord.Config;
using TtrpgAiBot.Platform.Discord.Extensions;

static void AddConfigSection<T>(WebApplicationBuilder builder, string sectionName)
    where T : class
{
  var configSection = builder.Configuration.GetSection(sectionName);
  builder.Services.Configure<T>(configSection);
  var config = configSection.Get<T>() ?? throw new InvalidOperationException($"{sectionName} section is missing or invalid in configuration.");
  builder.Services.AddSingleton(config);
}

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
AddConfigSection<AIConfig>(builder, "TtrpgAiBot:AI");
AddConfigSection<DiscordConfig>(builder, "TtrpgAiBot:Platforms:Discord");

var aiConfigSection = builder.Configuration.GetSection("TtrpgAiBot:AI");
builder.Services.Configure<AIConfig>(aiConfigSection);

// Safely bind DiscordConfig and handle missing config

// Add domain services
var discordConfig = builder.Configuration.Get<DiscordConfig>()!;
await builder.Services.AddDiscordIntegrationAsync(discordConfig);

builder.Services.AddProblemDetails();
builder.Services.AddBot();

var aiConfig = builder.Configuration.Get<AIConfig>()!;
builder.Services.AddAiServices(aiConfig);

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

  app.MapGet("/health", () => Results.Ok("Healthy"));

  app.MapControllers();

  app.Run();
  logger.LogInformation("TTRPG AI Bot API stopped gracefully.");
}
catch (Exception ex)
{
  logger.LogCritical(ex, "TTRPG AI Bot API terminated unexpectedly.");
  throw;
}
