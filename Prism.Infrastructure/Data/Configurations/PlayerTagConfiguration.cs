using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prism.Domain.Entities;

namespace Prism.Infrastructure.Data.Configurations;

public class PlayerTagConfiguration : IEntityTypeConfiguration<PlayerTag>
{
    public void Configure(EntityTypeBuilder<PlayerTag> builder)
    {
        builder.ToTable("PlayerTags");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PlayerName)
            .HasMaxLength(24)
            .IsRequired();

        builder.Property(x => x.PlayerNameNormalized)
            .HasMaxLength(24)
            .IsRequired();

        builder.Property(x => x.MapName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.GroupName)
            .HasMaxLength(50);

        builder.HasIndex(x => x.PlayerNameNormalized)
            .IsUnique();
    }
}