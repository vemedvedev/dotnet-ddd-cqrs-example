using Bank.Domain.TransactionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : BaseEntityTypeConfiguration,
    IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        AddDefaultColumns(builder);
        builder.ToTable("transaction");

        builder.Property(o => o.Uid)
            .HasColumnName("uid")
            .IsRequired();

        builder.Property(o => o.Type)
            .HasColumnName("type")
            .IsRequired();

        builder.HasIndex(o => o.Uid).IsUnique();

        builder.OwnsMany(t => t.TransactionLogs, tl =>
        {
            AddDefaultColumns(tl);
            tl.ToTable("transaction_log");

            tl.Property(p => p.Id).HasColumnName("id").IsRequired();

            tl.Property(p => p.TransactionId).HasColumnName("transaction_id");
            tl.Property(p => p.AccountId).HasColumnName("account_id");
            tl.Property(p => p.Delta).HasColumnName("delta");
            tl.Property(p => p.BeforeBalance).HasColumnName("before_balance");
            tl.Property(p => p.AfterBalance).HasColumnName("after_balance");

            tl.WithOwner().HasForeignKey(x => x.TransactionId);

            tl.HasKey(x => x.Id);
        });
    }
}
