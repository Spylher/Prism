using Prism.Application.Common;
using Prism.Application.Dtos;
using Prism.Application.Interfaces;
using Prism.Domain.Interfaces;

namespace Prism.Application.UseCases.Auth;

public class GetAppProfileDataUseCase
{
    private readonly IAppProfileRepository _appProfileRepository;
    private readonly IUnitOfWork _uow;

    public GetAppProfileDataUseCase(IAppProfileRepository appProfileRepository, IUnitOfWork uow)
    {
        _appProfileRepository = appProfileRepository;
        _uow = uow;
    }

    public async Task<Result<AppProfileResponse>> ExecuteAsync(Guid clientId, Guid profileId)
    {
        var appProfile = await _appProfileRepository.GetByIdAsync(profileId);

        if (appProfile == null)
            return Result<AppProfileResponse>.Fail("App profile not found", ErrorCode.NotFound);

        if (appProfile.ClientId != clientId)
            return Result<AppProfileResponse>.Fail("Access denied", ErrorCode.Forbidden);

        return Result<AppProfileResponse>.Ok(new AppProfileResponse(appProfile.Id, appProfile.Name, appProfile.Data));
    }
}