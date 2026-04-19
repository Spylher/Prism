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

        // Use cases
        service.AddScoped<LoginUseCase>();
        service.AddScoped<AddDaysToClientUseCase>();
        service.AddScoped<RefreshTokenUseCase>();
        return service;
    }
}
