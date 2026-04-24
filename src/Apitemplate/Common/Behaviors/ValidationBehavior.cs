using MediatR;

namespace Apitemplate.Common.Behaviors;

/// <summary>
/// Placeholder MediatR pipeline behavior for FluentValidation integration.
/// To activate validation:
/// 1. Add the <c>FluentValidation.AspNetCore</c> NuGet package.
/// 2. Register validators: <c>services.AddValidatorsFromAssembly(typeof(Program).Assembly);</c>
/// 3. Uncomment and complete the validation logic below.
/// </summary>
/// <typeparam name="TRequest">The MediatR request type.</typeparam>
/// <typeparam name="TResponse">The response type produced by the handler.</typeparam>
internal sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    /// <inheritdoc/>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // TODO: inject IEnumerable<IValidator<TRequest>> validators and run them here.
        // Example:
        //
        // var context = new ValidationContext<TRequest>(request);
        // var failures = validators
        //     .Select(v => v.Validate(context))
        //     .SelectMany(r => r.Errors)
        //     .Where(f => f != null)
        //     .ToList();
        //
        // if (failures.Count != 0)
        //     throw new ValidationException(failures);

        return await next(cancellationToken);
    }
}
