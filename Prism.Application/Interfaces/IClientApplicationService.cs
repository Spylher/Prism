using Prism.Application.Common;
using Prism.Application.Dtos;
namespace Prism.Application.Interfaces;

public interface IClientApplicationService
{
    Task<Result> RegisterAsync(RegisterClientRequest request);
    Task<Result> UpdateProfileAsync(UpdateClientRequest request);
    Task<Result> ChangePasswordAsync(string currentPassword, string newPassword);
    Task<Result> ResetPasswordAsync(Guid userId, string newPassword);
    Task<Result<ClientProfileDto>> GetProfileAsync();
}
