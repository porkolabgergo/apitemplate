namespace Apitemplate.Common.Results;

/// <summary>
/// Represents a domain or application error with a code and a human-readable message.
/// </summary>
/// <param name="Code">A short machine-readable identifier for the error (e.g. "WeatherForecast.NotFound").</param>
/// <param name="Message">A human-readable description of the error.</param>
public sealed record Error(string Code, string Message)
{
    /// <summary>A sentinel that represents "no error".</summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>Creates a not-found error.</summary>
    public static Error NotFound(string code, string message) => new(code, message);

    /// <summary>Creates a validation error.</summary>
    public static Error Validation(string code, string message) => new(code, message);

    /// <summary>Creates a conflict error.</summary>
    public static Error Conflict(string code, string message) => new(code, message);
}
