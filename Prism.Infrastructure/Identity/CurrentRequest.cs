using Microsoft.AspNetCore.Http;
using Prism.Domain.Interfaces;

public class CurrentRequest : ICurrentRequest
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentRequest(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetIpAddress()
    {
        var context = _httpContextAccessor.HttpContext;

        if (context == null)
            return "unknown";

        // ⚠️ suporte pra proxy / nginx
        var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

        if (!string.IsNullOrEmpty(ip))
            return ip.Split(',')[0];

        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}