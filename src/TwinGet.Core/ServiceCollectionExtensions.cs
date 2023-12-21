// This file is licensed to you under MIT license.

using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TwinGet.Core.Packaging;

namespace TwinGet.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient<IPackageService, PackageService>();

            return services;
        }
    }
}
