using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prism.Domain.Entities;

namespace Prism.Infrastructure.Data.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.RefreshTokenHash)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(s => s.DeviceFingerprint)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(s => s.DeviceName)
            .IsRequired()
            .HasMaxLength(100);

        // Índice para busca por RefreshToken ser O(1)
        builder.HasIndex(s => s.RefreshTokenHash).IsUnique();

        builder.HasIndex(s => s.ClientId);
    }
}