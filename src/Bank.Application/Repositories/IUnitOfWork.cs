namespace Bank.Application.Repositories;

public interface IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken ct);
}
