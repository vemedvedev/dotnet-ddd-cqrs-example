using Bank.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Persistence.Configurations;

public class BaseEntityTypeConfiguration
{
    protected static void AddDefaultColumns<T>(EntityTypeBuilder<T> entity)
        where T : BaseEntity
    {
        entity.Property(e => e.Id).HasColumnName("id").IsRequired();
        entity.HasKey(e => e.Id);

        entity.Property(e => e.CreatedAt).HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAdd().IsRequired();
        entity.HasIndex(e => e.CreatedAt);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    protected static void AddDefaultColumns<TEntity, TRelatedEntity>(
        OwnedNavigationBuilder<TEntity, TRelatedEntity> entity)
        where TRelatedEntity : BaseEntity
        where TEntity : class
    {
        entity.Property(e => e.CreatedAt).HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAdd().IsRequired();
    }
}
