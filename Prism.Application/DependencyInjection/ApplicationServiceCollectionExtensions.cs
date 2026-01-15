using Microsoft.Extensions.DependencyInjection;
using Prism.Application.Interfaces;
using Prism.Application.Services;
namespace Prism.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddScoped<IClientApplicationService, ClientApplicationService>();
        return service;
    }
}
