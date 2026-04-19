using Prism.Application.Common;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
using Prism.Domain.Common;
using Prism.Domain.Entities;
using Prism.Domain.Interfaces;

namespace Prism.Application.UseCases.Auth;

public class LoginUseCase
{
    private readonly IClientRepository _clientRepo;
    private readonly IAccountService _accountService;
    private readonly ISessionRepository _sessionRepo;
    private readonly ITokenService _tokenService;
    private readonly ITokenHashService _hashService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentRequest _currentRequest;
    public LoginUseCase(IAccountService accountService, ISessionRepository sessionRepo,
        ITokenService tokenService, IUnitOfWork unitOfWork, ITokenHashService hashService, IClientRepository clientRepo, ICurrentRequest currentRequest)
    {
        _accountService = accountService;
        _sessionRepo = sessionRepo;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _clientRepo = clientRepo;
        _currentRequest = currentRequest;
    }

    public async Task<Result<LoginClientResponse>> ExecuteAsync(LoginClientRequest req)
    {
        // 1. Valida credenciais via Identity
        var userResult = await _accountService.FindByEmailAsync(req.Email);
        if (!userResult.IsSuccess || userResult.Value is null)
            return Result<LoginClientResponse>.Fail("Invalid credentials.", ErrorCode.Unauthorized);

        var user = userResult.Value;

        var passwordValid = await _accountService.CheckPasswordAsync(user, req.Password);
        if (!passwordValid)
            return Result<LoginClientResponse>.Fail("Invalid credentials.", ErrorCode.Unauthorized);

        //var clientResult = await _clientService.GetProfileAsync()
        var client = await _clientRepo.GetByIdAsync(user.ClientId);
        if (client is null)
            return Result<LoginClientResponse>.Fail("Client not found.", ErrorCode.Unauthorized);

        // 2. Revoga sessões ativas anteriores do mesmo client (1 sessão por vez)
        var activeSessions = await _sessionRepo.GetActiveByClientIdAsync(user.ClientId);
        foreach (var s in activeSessions)
            s.Revoke(SessionRevocationReason.ReplacedByNewLogin);

        // 3. Gera tokens
        var roles = await _accountService.GetRolesAsync(user.Id);
        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.ClientId, user.FullName, user.Email, roles);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenHash = _hashService.Compute(refreshToken);
        var expiresAt = client.ExpiresAt;
        //var expiresAt = DateTime.UtcNow.AddDays(_tokenService.RefreshTokenExpirationDays);

        // 4. Cria e persiste a sessão
        var ipAddress = _currentRequest.GetIpAddress();
        var session = new Session(user.ClientId, accessToken, refreshTokenHash,
                                  req.DeviceFingerprint, req.DeviceName, ipAddress, expiresAt);

        await _sessionRepo.AddAsync(session);
        await _unitOfWork.CommitAsync();

        return Result<LoginClientResponse>.Ok(
            new LoginClientResponse(accessToken, refreshToken, expiresAt, user.FullName)
        );
    }
}