using Prism.Application.Common;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
using Prism.Domain.Entities;
using Prism.Domain.Interfaces;

namespace Prism.Application.UseCases.Auth;

public class UpdateAppProfileDataUseCase
{
    private readonly IClientRepository _clientRepository;
    private readonly IAppProfileRepository _appProfileRepository;
    private readonly IUnitOfWork _uow;

    public UpdateAppProfileDataUseCase(IAppProfileRepository appProfileRepository, IUnitOfWork uow, IClientRepository clientRepository)
    {
        _appProfileRepository = appProfileRepository;
        _clientRepository = clientRepository;
        _uow = uow;
    }

    public async Task<Result> ExecuteAsync(Guid clientId, UpdateAppProfileDataRequest updateAppProfileDataRequest)
    {
        var client = await _clientRepository.GetByIdAsync(clientId);

        if (client == null)
            return Result.Fail("Client not found", ErrorCode.ClientNotFound);

        var appProfiles = await _appProfileRepository.GetByClientIdAsync(clientId);
        var currentProfile = appProfiles.FirstOrDefault(p => p.Name.Equals(updateAppProfileDataRequest.Name, StringComparison.OrdinalIgnoreCase));
       
        if (currentProfile == null)
            return Result.Fail("Profile not found", ErrorCode.NotFound);

        currentProfile.Data = updateAppProfileDataRequest.Data.GetRawText();

        await _uow.CommitAsync();
        return Result.Ok();
    }

}
