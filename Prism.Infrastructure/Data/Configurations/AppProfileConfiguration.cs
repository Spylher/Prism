using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prism.Domain.Entities;
namespace Prism.Infrastructure.Data.Configurations;

public class AppProfileConfiguration : IEntityTypeConfiguration<AppProfile>
{
    public void Configure(EntityTypeBuilder<AppProfile> builder)
    {
        builder.ToTable("AppProfiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.HasIndex(s => s.ClientId);

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}