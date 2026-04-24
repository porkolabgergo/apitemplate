using Apitemplate.Common.Results;
using MediatR;

namespace Apitemplate.Features.WeatherForecast.CreateWeatherForecast;

/// <summary>
/// Command to create a new weather forecast entry.
/// </summary>
/// <param name="Date">The forecast date.</param>
/// <param name="TemperatureC">Temperature in Celsius.</param>
/// <param name="Summary">Optional summary.</param>
public sealed record CreateWeatherForecastCommand(
    DateOnly Date,
    int TemperatureC,
    string? Summary)
    : IRequest<Result<Guid>>;
