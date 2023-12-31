// This file is licensed to you under MIT license.

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TwinGet.Cli;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfigurationRoot config = configurationBuilder.Build();

        using ILoggerFactory loggerFactory = LoggerFactory.Create(loggingBuilder =>
        {
            loggingBuilder
                .ClearProviders()
                .AddConfiguration(config.GetSection("Logging"))
                .AddConsole();
        });

        ILogger logger = loggerFactory.CreateLogger(
            Assembly.GetExecutingAssembly().GetName().Name ?? "TwinGet"
        );

        services.AddSingleton(logger);

        return services;
    }
}
