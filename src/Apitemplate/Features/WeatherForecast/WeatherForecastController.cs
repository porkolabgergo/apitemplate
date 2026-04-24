using Apitemplate.Controllers;
using Apitemplate.Features.WeatherForecast.CreateWeatherForecast;
using Apitemplate.Features.WeatherForecast.GetWeatherForecast;
using Microsoft.AspNetCore.Mvc;

namespace Apitemplate.Features.WeatherForecast;

/// <summary>
/// Handles HTTP requests related to weather forecasts.
/// </summary>
public sealed class WeatherForecastController : BaseApiController
{
    /// <summary>
    /// Retrieves a weather forecast for the specified number of days ahead.
    /// </summary>
    /// <param name="daysAhead">Number of forecast days (1–10). Defaults to 5.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of weather forecast entries.</returns>
    /// <response code="200">The forecast was retrieved successfully.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet(Name = "GetWeatherForecast")]
    [ProducesResponseType(typeof(IReadOnlyList<GetWeatherForecastResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(
        [FromQuery] int daysAhead = 5,
        CancellationToken cancellationToken = default)
    {
        var query = new GetWeatherForecastQuery(daysAhead);
        var result = await Sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Creates a new weather forecast entry.
    /// </summary>
    /// <param name="request">The forecast data to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the newly created forecast.</returns>
    /// <response code="201">The forecast was created successfully.</response>
    /// <response code="400">The request data was invalid.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpPost(Name = "CreateWeatherForecast")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateWeatherForecastRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateWeatherForecastCommand(
            request.Date,
            request.TemperatureC,
            request.Summary);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return HandleResult(result);

        return CreatedResult(
            "GetWeatherForecast",
            new { },
            result.Value);
    }
}
