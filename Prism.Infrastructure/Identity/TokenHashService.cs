using System.Security.Cryptography;
using System.Text;
using Prism.Domain.Interfaces;
using Prism.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Prism.Infrastructure.Security;

public class TokenHashService : ITokenHashService
{
    private readonly string _secret;

    public TokenHashService(IOptions<JwtSettings> settings)
    {
        _secret = settings.Value.SecretKey;
    }

    public string Compute(string input)
    {
        var key = Encoding.UTF8.GetBytes(_secret);
        using var hmac = new HMACSHA256(key);

        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(hash);
    }
}