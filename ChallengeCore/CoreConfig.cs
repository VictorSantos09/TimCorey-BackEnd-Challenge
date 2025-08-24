using ChallengeCore.Application.Services.Products;
using ChallengeCore.Application.Services.Purchases;
using ChallengeCore.Application.Services.Users;
using ChallengeCore.Extensions;
using ChallengeCore.Infrastructure.Data;
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
        builder.Services.AddControllers();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        if (builder.Environment.IsProduction())
        {
            builder.WebHost.UseUrls("http://*:80");
        }

        AddLogging(builder);

        var connectionString = builder.Configuration.GetRequiredConnectionString();

        builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        builder.Services.AddScoped<DbContext, AppDbContext>();

        builder.Services
            .AddRepositories()
            .AddServices();
    }

    public static void AddCore(this WebApplication app)
    {
        app.MapControllers();

        app.UseExceptionHandler();
        app.UseSerilogRequestLogging();

        var logger = app.Services.GetRequiredService<ILogger<CoreConfigLogger>>();

        var retryPolicy = Polly.Policy
        .Handle<Exception>()
        .WaitAndRetry(15, i => TimeSpan.FromSeconds(5), (ex, ts) =>
        {
            logger.LogWarning("Tentativa de conexão ao banco falhou {ex}", ex);
        });

        using (var scope = app.Services.CreateScope())
        {
            retryPolicy.Execute(() =>
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (db.Database.CanConnect())
                {
                    logger.LogInformation("Aplicando migration.");
                    db.Database.Migrate();
                    logger.LogInformation("Migration aplicada com sucesso.");
                }
                else
                {
                    logger.LogError("Não foi possível conectar ao banco de dados.");
                }
            });
        }
    }
    private static void AddLogging(WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration
                 .ReadFrom.Configuration(context.Configuration)
                 .Enrich.FromLogContext();
        });

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("ChallengeAPI").AddService("connector-net"))
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation();
                metrics.AddHttpClientInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddEntityFrameworkCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();

                tracing.AddConnectorNet();
                tracing.AddOtlpExporter();
            });

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.ParseStateValues = true;
            logging.IncludeScopes = true;

            logging
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
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPurchaseRepository, PurchaseRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IPurchaseService, PurchaseService>();

        return services;
    }
}
