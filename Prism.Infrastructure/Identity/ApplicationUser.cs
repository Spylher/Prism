using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace Prism.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid ClientId { get; set; }
    public required string FullName { get; set; }
}
