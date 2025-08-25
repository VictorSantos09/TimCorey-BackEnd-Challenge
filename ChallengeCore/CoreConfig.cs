using ChallengeCore.Application.Services.Products;
using ChallengeCore.Application.Services.Purchases;
using ChallengeCore.Application.Services.Users;
using ChallengeCore.Extensions;
using ChallengeCore.Infrastructure.Data;
using ChallengeCore.Infrastructure.Logging;
using ChallengeCore.Infrastructure.Repository;
using ChallengeCore.Infrastructure.Repository.Abstractions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using Polly;

using Serilog;

namespace ChallengeCore;

public class CoreConfigLogger;

public static class CoreConfig
{
    public static void AddCore(this WebApplicationBuilder builder)
    {
        _ = builder.Services.AddControllers();

        _ = builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        _ = builder.Services.AddProblemDetails();

        if (builder.Environment.IsProduction())
        {
            _ = builder.WebHost.UseUrls("http://*:80");
        }

        AddLogging(builder);

        string connectionString = builder.Configuration.GetRequiredConnectionString();

        _ = builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        _ = builder.Services.AddScoped<DbContext, AppDbContext>();

        _ = builder.Services
            .AddRepositories()
            .AddServices();
    }

    public static void AddCore(this WebApplication app)
    {
        _ = app.MapControllers();

        _ = app.UseExceptionHandler();
        _ = app.UseSerilogRequestLogging();

        ILogger<CoreConfigLogger> logger = app.Services.GetRequiredService<ILogger<CoreConfigLogger>>();

        Polly.Retry.RetryPolicy retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetry(15, i => TimeSpan.FromSeconds(5), (ex, ts) =>
        {
            logger.Warning("Tentativa de conexão ao banco falhou {ex}", ex);
        });

        using IServiceScope scope = app.Services.CreateScope();
        retryPolicy.Execute(() =>
        {
            AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (db.Database.CanConnect())
            {
                logger.Information("Aplicando migration.");
                db.Database.Migrate();
                logger.Information("Migration aplicada com sucesso.");
            }
            else
            {
                logger.Error("Não foi possível conectar ao banco de dados.");
            }
        });
    }
    private static void AddLogging(WebApplicationBuilder builder)
    {
        _ = builder.Logging.ClearProviders();

        _ = builder.Host.UseSerilog((context, configuration) =>
        {
            _ = configuration
                 .ReadFrom.Configuration(context.Configuration)
                 .Enrich.FromLogContext();
        });

        _ = builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("ChallengeAPI").AddService("connector-net"))
            .WithMetrics(metrics =>
            {
                _ = metrics.AddAspNetCoreInstrumentation();
                _ = metrics.AddHttpClientInstrumentation();
            })
            .WithTracing(tracing =>
            {
                _ = tracing.AddAspNetCoreInstrumentation();
                _ = tracing.AddEntityFrameworkCoreInstrumentation();
                _ = tracing.AddHttpClientInstrumentation();

                _ = tracing.AddConnectorNet();
                _ = tracing.AddOtlpExporter();
            });

        _ = builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.ParseStateValues = true;
            logging.IncludeScopes = true;

            _ = logging
            .AddOtlpExporter(a =>
            {
                a.Endpoint = new Uri("http://seq:5341/ingest/otlp/v1/logs");
                a.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
            })
            .AddConsoleExporter();
        });
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        _ = services.AddScoped<IUserRepository, UserRepository>();
        _ = services.AddScoped<IProductRepository, ProductRepository>();
        _ = services.AddScoped<IPurchaseRepository, PurchaseRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        _ = services.AddScoped<IUserService, UserService>();
        _ = services.AddScoped<IProductService, ProductService>();
        _ = services.AddScoped<IPurchaseService, PurchaseService>();

        return services;
    }
}
