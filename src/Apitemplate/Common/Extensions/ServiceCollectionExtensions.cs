using Apitemplate.Common.Behaviors;
using Apitemplate.Common.Exceptions;
using Microsoft.OpenApi.Models;

namespace Apitemplate.Common.Extensions;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to register all application services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all application services including MediatR, behaviors, and exception handling.
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    /// <summary>
    /// Registers Swagger/OpenAPI services.
    /// </summary>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ApiTemplate",
                Version = "v1",
                Description = "A production-ready .NET 10 Web API template using CQRS with vertical slices.",
            });
        });

        return services;
    }
}
