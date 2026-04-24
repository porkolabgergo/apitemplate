namespace Apitemplate.Features.WeatherForecast.GetWeatherForecast;

/// <summary>
/// Response DTO returned by the <c>GetWeatherForecast</c> query.
/// </summary>
/// <param name="Date">The forecast date.</param>
/// <param name="TemperatureC">Temperature in Celsius.</param>
/// <param name="TemperatureF">Temperature in Fahrenheit (derived).</param>
/// <param name="Summary">A short human-readable summary.</param>
public sealed record GetWeatherForecastResponse(
    DateOnly Date,
    int TemperatureC,
    int TemperatureF,
    string? Summary);
