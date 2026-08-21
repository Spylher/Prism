using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prism.Domain.Entities;
namespace Prism.Infrastructure.Data.Configurations;

public class DiscordProfileConfiguration : IEntityTypeConfiguration<DiscordProfile>
{
    public void Configure(EntityTypeBuilder<DiscordProfile> builder)
    {
        builder.ToTable("DiscordProfiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DiscordUserId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.DiscordGlobalName)
            .HasMaxLength(100);

        builder.Property(x => x.DiscordNickName)
            .HasMaxLength(100);

        builder.Property(x => x.DiscordAvatarHash)
            .HasMaxLength(500);

        builder.HasIndex(s => s.ClientId);

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}