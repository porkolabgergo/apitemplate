using Apitemplate.Common.Results;
using MediatR;

namespace Apitemplate.Features.WeatherForecast.GetWeatherForecast;

/// <summary>
/// Handles the <see cref="GetWeatherForecastQuery"/> by returning a stub in-memory forecast.
/// Replace this with a real repository call when connecting to a database.
/// </summary>
internal sealed class GetWeatherForecastQueryHandler
    : IRequestHandler<GetWeatherForecastQuery, Result<IReadOnlyList<GetWeatherForecastResponse>>>
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
    ];

    /// <inheritdoc/>
    public Task<Result<IReadOnlyList<GetWeatherForecastResponse>>> Handle(
        GetWeatherForecastQuery request,
        CancellationToken cancellationToken)
    {
        int days = Math.Clamp(request.DaysAhead, 1, 10);

        IReadOnlyList<GetWeatherForecastResponse> forecast = Enumerable
            .Range(1, days)
            .Select(index =>
            {
                int tempC = Random.Shared.Next(-20, 55);
                return new GetWeatherForecastResponse(
                    Date: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(index)),
                    TemperatureC: tempC,
                    TemperatureF: 32 + (int)(tempC / 0.5556),
                    Summary: Summaries[Random.Shared.Next(Summaries.Length)]);
            })
            .ToList()
            .AsReadOnly();

        return Task.FromResult(Result.Success(forecast));
    }
}
