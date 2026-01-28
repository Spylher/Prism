using FluentValidation;
using Prism.Application.Common;
using Prism.Application.Dtos;
using Prism.Application.Extensions;
using Prism.Application.Interfaces;
using Prism.Domain.Entities;
using Prism.Domain.Exceptions;
using Prism.Domain.Interfaces;
namespace Prism.Application.Services;

public class ClientApplicationService : IClientApplicationService
{
    private readonly IClientRepository _clientRepo;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountService _accountService;
    private readonly IValidator<RegisterClientRequest> _registerClientValidator;

    public ClientApplicationService(IClientRepository clientRepo, IAccountService account, ICurrentUser currentUser, IUnitOfWork unitOfWork, IValidator<RegisterClientRequest> registerClientValidator)
    {
        _clientRepo = clientRepo;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
        _registerClientValidator = registerClientValidator;
        _accountService = account;
    }

    public async Task<Result> RegisterAsync(RegisterClientRequest req)
    {
        var validation = await _registerClientValidator.ValidateAsync(req);
        if (!validation.IsValid)
            return validation.ToResult();

        var client = new Client(req.FirstName, req.LastName);

        await _clientRepo.AddAsync(client);

        var accountResult = await _accountService.CreateUserAsync(client.Id, $"{req.FirstName} {req.LastName}", req.Email, req.Password);

        if (accountResult.IsSuccess) 
            return Result.Ok();
        
        _clientRepo.Remove(client);
        return Result.Fail(accountResult.Error ?? "Error on create account.", ErrorCode.Conflict);
    }

    public async Task<Result> UpdateProfileAsync(UpdateClientRequest request)
    {
        var clientId = await _currentUser.GetClientIdAsync();
        if (clientId is null)
            return Result.Fail("User not logged.", ErrorCode.Unauthorized);

        var client = await _clientRepo.GetByIdAsync(clientId.Value);
        if (client == null)
            return Result.Fail("User not found.", ErrorCode.NotFound);

        try
        {
            client.UpdateName(request.FirstName, request.LastName);
            client.SetActiveStatus(request.IsActive);

            await _unitOfWork.CommitAsync();
            return Result.Ok();
        }
        catch (DomainException ex)
        {
            return Result.Fail(ex.Message, ErrorCode.ValidationError);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Fatal Error: {ex.Message}", ErrorCode.InfrastructureError);
        }
    }

    public async Task<Result> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        var userId = await _currentUser.GetUserIdAsync();
        if (userId is null)
            return Result.Fail("User not logged.", ErrorCode.Unauthorized);

        return await _accountService.ChangePasswordByUserIdAsync(userId.Value, currentPassword, newPassword);
    }

    public async Task<Result> ResetPasswordAsync(Guid userId, string newPassword)
    {
        return await _accountService.ResetPasswordByUserIdAsync(userId, newPassword);
    }

    public async Task<Result<ClientProfileDto>> GetProfileAsync()
    {
        var userResult = await _currentUser.GetUserAsync();
        if (!userResult.IsSuccess || userResult.Value is null)
            return Result<ClientProfileDto>.Fail(userResult.Error ?? "User not found.", ErrorCode.NotFound);

        var userReadModel = userResult.Value;
        var client = await _clientRepo.GetByIdAsync(userReadModel.ClientId);
        if (client is null)
            return Result<ClientProfileDto>.Fail("Client not found.", ErrorCode.NotFound);

        return Result<ClientProfileDto>.Ok(ClientProfileDto.FromDomain(userReadModel, client));
    }
}

