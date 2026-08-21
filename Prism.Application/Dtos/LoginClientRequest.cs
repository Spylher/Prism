namespace Prism.Application.Dtos;

public record LoginClientRequest(
    string Email, 
    string Password, 
    string DeviceFingerprint, 
    string DeviceName,
    string WindowsUser,
    string MacAddress,
    bool RememberMe = true
);
