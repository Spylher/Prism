namespace Prism.Application.Dtos;

public record LoginClientRequest(
    string Email, 
    string Password, 
    string DeviceFingerprint, 
    string DeviceName, 
    bool RememberMe = true
);
