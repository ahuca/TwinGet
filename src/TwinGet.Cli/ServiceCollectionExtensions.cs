// This file is licensed to you under MIT license.

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TwinGet.Cli
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLogger(this IServiceCollection services)
        {
            string appsettingsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "appsettings.json");

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile(
                appsettingsPath,
                optional: false,
                reloadOnChange: true);

            IConfigurationRoot config = configurationBuilder.Build();

            using ILoggerFactory loggerFactory = LoggerFactory.Create(loggingBuilder =>
            {
                loggingBuilder
                .ClearProviders()
                .AddConfiguration(config.GetSection("Logging"))
                .AddConsole();
            });

            ILogger logger = loggerFactory.CreateLogger(Assembly.GetExecutingAssembly().GetName().Name ?? "TwinGet");

            services.AddSingleton(logger);

            return services;
        }
    }
}
