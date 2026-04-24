using Apitemplate.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Apitemplate.Controllers;

/// <summary>
/// Base controller that all API controllers should inherit from.
/// Provides MediatR <see cref="ISender"/> access and a rich set of HTTP response helpers
/// that produce RFC 7807 <see cref="ProblemDetails"/> on error.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private ISender? _sender;

    /// <summary>
    /// Gets the MediatR <see cref="ISender"/> resolved from the DI container.
    /// Lazily resolved so derived controllers don't need constructor injection.
    /// </summary>
    protected ISender Sender =>
        _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    // ─── 2xx ────────────────────────────────────────────────────────────────

    /// <summary>Returns HTTP 200 OK with the provided data.</summary>
    protected IActionResult OkResult<T>(T data) => Ok(data);

    /// <summary>Returns HTTP 201 Created with a location header and the created resource.</summary>
    protected IActionResult CreatedResult<T>(string routeName, object routeValues, T data) =>
        CreatedAtRoute(routeName, routeValues, data);

    /// <summary>Returns HTTP 202 Accepted.</summary>
    protected IActionResult AcceptedResult() => Accepted();

    /// <summary>Returns HTTP 204 No Content.</summary>
    protected IActionResult NoContentResult() => NoContent();

    // ─── 4xx ────────────────────────────────────────────────────────────────

    /// <summary>Returns HTTP 400 Bad Request as ProblemDetails.</summary>
    protected IActionResult BadRequestResult(string detail) =>
        Problem(
            detail: detail,
            statusCode: StatusCodes.Status400BadRequest,
            title: "Bad Request",
            type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1");

    /// <summary>Returns HTTP 404 Not Found as ProblemDetails.</summary>
    protected IActionResult NotFoundResult(string detail) =>
        Problem(
            detail: detail,
            statusCode: StatusCodes.Status404NotFound,
            title: "Not Found",
            type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4");

    /// <summary>Returns HTTP 409 Conflict as ProblemDetails.</summary>
    protected IActionResult ConflictResult(string detail) =>
        Problem(
            detail: detail,
            statusCode: StatusCodes.Status409Conflict,
            title: "Conflict",
            type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8");

    /// <summary>Returns HTTP 422 Unprocessable Entity as ProblemDetails.</summary>
    protected IActionResult UnprocessableResult(string detail) =>
        Problem(
            detail: detail,
            statusCode: StatusCodes.Status422UnprocessableEntity,
            title: "Unprocessable Entity",
            type: "https://datatracker.ietf.org/doc/html/rfc4918#section-11.2");

    // ─── Result<T> mapping ──────────────────────────────────────────────────

    /// <summary>
    /// Maps a <see cref="Result{T}"/> to the appropriate HTTP response.
    /// Returns 200 OK with the value on success, or a 4xx/5xx ProblemDetails on failure.
    /// </summary>
    protected IActionResult HandleResult<T>(Result<T> result) =>
        result.IsSuccess ? OkResult(result.Value) : HandleFailure(result);

    /// <summary>
    /// Maps a <see cref="Result"/> (non-generic) to the appropriate HTTP response.
    /// Returns 204 No Content on success, or a 4xx/5xx ProblemDetails on failure.
    /// </summary>
    protected IActionResult HandleResult(Result result) =>
        result.IsSuccess ? NoContentResult() : HandleFailure(result);

    private IActionResult HandleFailure(Result result)
    {
        string code = result.Error.Code;
        string message = result.Error.Message;

        return code switch
        {
            var c when c.EndsWith(".NotFound", StringComparison.OrdinalIgnoreCase) =>
                NotFoundResult(message),
            var c when c.EndsWith(".Conflict", StringComparison.OrdinalIgnoreCase) =>
                ConflictResult(message),
            var c when c.EndsWith(".Validation", StringComparison.OrdinalIgnoreCase) =>
                UnprocessableResult(message),
            _ => BadRequestResult(message),
        };
    }
}
