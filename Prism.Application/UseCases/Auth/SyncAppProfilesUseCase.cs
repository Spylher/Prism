using Prism.Application.Common;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
using Prism.Domain.Entities;
using Prism.Domain.Interfaces;

namespace Prism.Application.UseCases.Auth;

public class SyncAppProfilesUseCase
{
    private readonly IClientRepository _clientRepository;
    private readonly IAppProfileRepository _appProfileRepository;
    private readonly IUnitOfWork _uow;

    public SyncAppProfilesUseCase(IAppProfileRepository appProfileRepository, IUnitOfWork uow, IClientRepository clientRepository)
    {
        _appProfileRepository = appProfileRepository;
        _clientRepository = clientRepository;
        _uow = uow;
    }

    public async Task<Result> ExecuteAsync(Guid clientId, SyncProfilesRequest syncProfilesRequest)
    {
        var client = await _clientRepository.GetByIdAsync(clientId);

        if (client == null)
            return Result.Fail("Client not found.", ErrorCode.ClientNotFound);

        var appProfiles = await _appProfileRepository.GetByClientIdAsync(clientId);
        var newAppProfiles = syncProfilesRequest.Profiles.ToHashSet(StringComparer.OrdinalIgnoreCase);

        // Remove profiles that are not in the request
        var profilesToRemove = appProfiles
            .Where(profile => !newAppProfiles.Contains(profile.Name))
            .ToList();

        appProfiles.RemoveAll(profile => !newAppProfiles.Contains(profile.Name));
        _appProfileRepository.RemoveRange(profilesToRemove);

        // Get existing profile names after removals
        var existingProfileNames = appProfiles
            .Select(profile => profile.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        // Add new profiles from the request that are not in the existing profiles
        foreach (var profileName in newAppProfiles)
        {
            if (existingProfileNames.Contains(profileName))
                continue;

            await _appProfileRepository.AddAsync(new AppProfile
            {
                Id = Guid.NewGuid(),
                Name = profileName,
                ClientId = clientId
            });
        }

        await _uow.CommitAsync();
        return Result.Ok();
    }

}
