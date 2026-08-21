using Prism.Application.Common;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
using Prism.Domain.Entities;
using Prism.Domain.Interfaces;
namespace Prism.Application.UseCases.Auth;

public class SyncDiscordUseCase
{
    private readonly IClientRepository _clientRepository;
    private readonly IDiscordProfileRepository _discordProfileRepository;
    private readonly IUnitOfWork _uow;

    public SyncDiscordUseCase(IClientRepository repo, IUnitOfWork uow, IDiscordProfileRepository discordProfileRepository)
    {
        _discordProfileRepository = discordProfileRepository;
        _clientRepository = repo;
        _uow = uow;
    }

    public async Task<Result> ExecuteAsync(Guid clientId, DiscordProfileRequest discordProfileRequest)
    {
        var client = await _clientRepository.GetByIdAsync(clientId);

        if (client == null)
            return Result.Fail("Client not found", ErrorCode.ClientNotFound);

        var discordProfiles = await _discordProfileRepository.GetByClientIdAsync(clientId);
        var currentProfile = discordProfiles.FirstOrDefault(p => p.DiscordUserId == discordProfileRequest.UserId);

        if (currentProfile != null)
        {
            // Update existing profile
            currentProfile.DiscordNickName = discordProfileRequest.NickName;
            currentProfile.DiscordGlobalName = discordProfileRequest.GlobalName;
            currentProfile.DiscordAvatarHash = discordProfileRequest.AvatarHash;
        }
        else
        {
            // Create new profile
            var discordProfile = new DiscordProfile
            {
                Id = Guid.NewGuid(),
                DiscordUserId = discordProfileRequest.UserId,
                DiscordNickName = discordProfileRequest.NickName,
                DiscordGlobalName = discordProfileRequest.GlobalName,
                DiscordAvatarHash = discordProfileRequest.AvatarHash
            };

            foreach (var profile in discordProfiles)
                profile.Revoke();

            await _discordProfileRepository.AddAsync(discordProfile);
        }

        await _uow.CommitAsync();
        return Result.Ok();
    }

}