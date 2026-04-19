using Prism.Application.Common;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
using Prism.Domain.Entities;
using Prism.Domain.Interfaces;

namespace Prism.Application.UseCases.Auth;

public class RefreshTokenUseCase
{
    private readonly ISessionRepository _sessionRepo;
    private readonly IAccountService _accountService;
    private readonly ITokenService _tokenService;
    private readonly ITokenHashService _hashService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientRepository _clientRepo;
    private readonly ICurrentRequest _currentRequest;

    public RefreshTokenUseCase(ISessionRepository sessionRepo, IAccountService accountService, ITokenService tokenService, ITokenHashService hashService, IUnitOfWork unitOfWork, IClientRepository clientRepo, ICurrentRequest currentRequest)
    {
        _sessionRepo = sessionRepo;
        _accountService = accountService;
        _tokenService = tokenService;
        _hashService = hashService;
        _unitOfWork = unitOfWork;
        _clientRepo = clientRepo;
        _currentRequest = currentRequest;
    }

    public async Task<Result<LoginClientResponse>> ExecuteAsync(RefreshTokenRequest req)
    {
        var hash = _hashService.Compute(req.RefreshToken);
        var session = await _sessionRepo.GetByRefreshTokenHashAsync(hash);

        if (session is null || !session.IsActive)
            return Result<LoginClientResponse>.Fail("Session expired or invalid.", ErrorCode.Unauthorized);
        
        // Garante que é o mesmo dispositivo
        if (session.DeviceFingerprint != req.DeviceFingerprint)
            return Result<LoginClientResponse>.Fail("Device mismatch.", ErrorCode.Unauthorized);

        var userResult = await _accountService.FindByClientIdAsync(session.ClientId);
        if (!userResult.IsSuccess || userResult.Value is null)
            return Result<LoginClientResponse>.Fail("User not found.", ErrorCode.NotFound);

        var client = await _clientRepo.GetByIdAsync(userResult.Value.ClientId);
        if (client is null)
            return Result<LoginClientResponse>.Fail("Client not found.", ErrorCode.Unauthorized);

        var user = userResult.Value;
        var roles = await _accountService.GetRolesAsync(user.Id);
        var newAccess = _tokenService.GenerateAccessToken(user.Id, user.ClientId, user.FullName, user.Email, roles);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        var newRefreshTokenHash = _hashService.Compute(newRefreshToken);
        var newExpires = client.ExpiresAt;
        //var newExpires = DateTime.UtcNow.AddDays(_tokenService.RefreshTokenExpirationDays);

        var ip = _currentRequest.GetIpAddress();
        session.RotateTokens(newAccess, newRefreshTokenHash, newExpires);
        session.UpdateLastIp(ip);
        await _unitOfWork.CommitAsync();

        return Result<LoginClientResponse>.Ok(
            new LoginClientResponse(newAccess, newRefreshToken, newExpires, user.FullName)
        );
    }
}