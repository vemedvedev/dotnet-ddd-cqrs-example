namespace Bank.Application.Handlers;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    public Task<TResult> HandleAsync(TQuery input, CancellationToken ct);
}
