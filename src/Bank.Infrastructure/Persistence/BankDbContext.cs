using Bank.Domain.AccountAggregate;
using Bank.Domain.TransactionAggregate;
using Bank.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Persistence;

public class BankDbContext : DbContext
{
    public DbSet<Account> Account => Set<Account>();
    public DbSet<AccountBalance> AccountBalance => Set<AccountBalance>();
    public DbSet<Transaction> Transaction => Set<Transaction>();

    public BankDbContext(DbContextOptions<BankDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("uuid-ossp");
        modelBuilder.HasPostgresExtension("citext");

        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
    }
}
