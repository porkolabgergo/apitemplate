using Apitemplate.Common.Results;
using MediatR;

namespace Apitemplate.Features.WeatherForecast.GetWeatherForecast;

/// <summary>
/// Query that retrieves the 5-day weather forecast.
/// </summary>
/// <param name="DaysAhead">Number of days to forecast (1–10).</param>
public sealed record GetWeatherForecastQuery(int DaysAhead = 5)
    : IRequest<Result<IReadOnlyList<GetWeatherForecastResponse>>>;
