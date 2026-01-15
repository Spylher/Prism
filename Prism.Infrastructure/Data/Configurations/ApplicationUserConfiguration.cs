using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prism.Infrastructure.Identity;
namespace Prism.Infrastructure.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasIndex(u => u.ClientId)
            .IsUnique();

        builder.Property(u => u.ClientId)
            .IsRequired();
    }
}
