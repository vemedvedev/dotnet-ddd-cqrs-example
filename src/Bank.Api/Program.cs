using Bank.Api;
using Bank.Application;
using Bank.Infrastructure;
using Bank.Infrastructure.Persistence;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Host.UseSerilog((ctx, config) =>
{
    config.ReadFrom.Configuration(ctx.Configuration);
});

builder.Services.ConfigureApiLayer();
builder.Services.ConfigureApplicationLayer();
builder.Services.ConfigureInfrastructureLayer(builder.Configuration);
builder.Services.AddControllers();

if (!builder.Environment.IsProduction())
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.DescribeAllParametersInCamelCase();
        c.EnableAnnotations(); // Enable annotations
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bank.Api", Version = "v1" });
    });
}

var app = builder.Build();
app.MapHealthChecks("/healthz",new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

ApplyMigrations(app);
app.Run();

void ApplyMigrations(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<BankDbContext>();

    // Check and apply pending migrations
    var pendingMigrations = dbContext.Database.GetPendingMigrations();
    if (pendingMigrations.Any())
    {
        dbContext.Database.Migrate();
    }
    else
    {
        Console.WriteLine("No pending migrations found.");
    }
}

// For integration tests
namespace Bank.Api
{
    public partial class Program;
}
