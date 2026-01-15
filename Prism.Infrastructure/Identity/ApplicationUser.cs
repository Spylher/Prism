using Microsoft.AspNetCore.Identity;
namespace Prism.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid ClientId { get; set; }
}
