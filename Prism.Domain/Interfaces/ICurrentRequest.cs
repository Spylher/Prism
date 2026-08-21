namespace Prism.Domain.Interfaces;

public interface ICurrentRequest
{
    string GetIpAddress();
    string? GetHeader(string key);
}