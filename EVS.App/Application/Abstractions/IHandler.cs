using EVS.App.Domain.Abstractions;

namespace EVS.App.Application.Abstractions;

/// <summary>
/// Handler for processing <typeparamref name="TRequest"/>.
/// </summary>
/// <typeparam name="TResponse"> Response type. Must inherit from <see cref="Result"/></typeparam>
/// <typeparam name="TRequest"> Input request type. </typeparam>
public interface IHandler<TResponse, in TRequest>
    where TResponse : Result 
    where TRequest : class
{
    Task<TResponse> Handle(
        TRequest request = default!,
        CancellationToken cancellationToken = default);
}