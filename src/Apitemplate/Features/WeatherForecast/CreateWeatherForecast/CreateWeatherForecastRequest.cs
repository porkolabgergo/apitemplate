namespace Apitemplate.Features.WeatherForecast.CreateWeatherForecast;

/// <summary>
/// Input DTO for creating a new weather forecast entry.
/// </summary>
/// <param name="Date">The forecast date (must be in the future).</param>
/// <param name="TemperatureC">Temperature in Celsius.</param>
/// <param name="Summary">A short summary (optional).</param>
public sealed record CreateWeatherForecastRequest(
    DateOnly Date,
    int TemperatureC,
    string? Summary);
