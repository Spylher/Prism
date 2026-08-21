using Microsoft.Extensions.DependencyInjection;
using Prism.Application.Interfaces;
using Prism.Application.Services;
using Prism.Application.UseCases.Auth;

namespace Prism.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddScoped<IClientApplicationService, ClientApplicationService>();
        service.AddScoped<ISessionApplicationService, SessionApplicationService>();
        service.AddScoped<IPlayerTagRealtimeService, PlayerTagRealtimeService>();

        // Use cases
        service.AddScoped<LoginUseCase>();
        service.AddScoped<AddDaysToClientUseCase>();
        service.AddScoped<SyncDiscordUseCase>();
        service.AddScoped<SyncAppProfilesUseCase>();
        service.AddScoped<UpdateAppProfileDataUseCase>();
        service.AddScoped<RefreshTokenUseCase>();
        service.AddScoped<GetAppProfilesUseCase>();
        service.AddScoped<GetAppProfileDataUseCase>();
        return service;
    }
}
