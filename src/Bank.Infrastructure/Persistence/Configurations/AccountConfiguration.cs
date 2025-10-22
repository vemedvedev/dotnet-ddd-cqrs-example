using Bank.Domain.AccountAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : BaseEntityTypeConfiguration,
    IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        AddDefaultColumns(builder);
        builder.ToTable("account");

        builder.Property(o => o.Name).HasColumnName("name")
            .HasColumnType("CITEXT")
            .IsRequired().HasMaxLength(256);

        builder.Property(o => o.Uid).HasColumnName("uid")
            .IsRequired();

        builder.OwnsOne<AccountBalance>(a => a.AccountBalance, ab =>
        {
            AddDefaultColumns(ab);
            ab.ToTable("account_balance");
            ab.Property(p => p.Id).HasColumnName("id").IsRequired();

            ab.Property(p => p.Balance).HasColumnName("balance");
            ab.Property(p => p.AccountId).HasColumnName("account_id");

            ab.Property(b => b.Version)
                .IsRowVersion();

            ab.WithOwner().HasForeignKey(ct => ct.AccountId);

            ab.HasKey(oi => oi.Id);
        });

        builder.HasIndex(o => o.Name).IsUnique();
        builder.HasIndex(o => o.Uid).IsUnique();
    }
}
