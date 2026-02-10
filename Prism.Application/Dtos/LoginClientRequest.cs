namespace Prism.Application.Dtos;

public record LoginClientRequest(string Email, string Password, bool RememberMe);
