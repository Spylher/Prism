using Prism.Application.Common;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
using Prism.Domain.Interfaces;
namespace Prism.Application.UseCases.Auth;

public class GetAppProfilesUseCase
{
    private readonly IAppProfileRepository _appProfileRepository;
    private readonly IUnitOfWork _uow;

    public GetAppProfilesUseCase(IAppProfileRepository appProfileRepository, IUnitOfWork uow)
    {
        _appProfileRepository = appProfileRepository;
        _uow = uow;
    }

    public async Task<IEnumerable<AppProfileResponse>> ExecuteAsync(Guid clientId)
    {
        var appProfiles = await _appProfileRepository.GetByClientIdAsync(clientId);
        return appProfiles.Select(profile => new AppProfileResponse(profile.Id, profile.Name));
    }
}



