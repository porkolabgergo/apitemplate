using Apitemplate.Common.Results;
using MediatR;

namespace Apitemplate.Features.WeatherForecast.CreateWeatherForecast;

/// <summary>
/// Handles the <see cref="CreateWeatherForecastCommand"/>.
/// This is a stub implementation — replace the body with actual persistence logic
/// using <c>IRepository&lt;WeatherForecastEntity&gt;</c> and <c>IUnitOfWork</c>.
/// </summary>
internal sealed class CreateWeatherForecastCommandHandler
    : IRequestHandler<CreateWeatherForecastCommand, Result<Guid>>
{
    /// <inheritdoc/>
    public Task<Result<Guid>> Handle(
        CreateWeatherForecastCommand request,
        CancellationToken cancellationToken)
    {
        // Stub: generate a new ID to represent the created entity.
        // In a real implementation you would:
        // 1. Map the command to an entity.
        // 2. Persist it via IRepository<WeatherForecastEntity>.AddAsync(...).
        // 3. Commit via IUnitOfWork.SaveChangesAsync(...).
        // 4. Return the new entity's ID.

        var newId = Guid.NewGuid();
        return Task.FromResult(Result.Success(newId));
    }
}
