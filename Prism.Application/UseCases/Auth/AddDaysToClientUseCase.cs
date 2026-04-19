using Prism.Application.Common;
using Prism.Application.Interfaces;
using Prism.Domain.Interfaces;
namespace Prism.Application.UseCases.Auth;

public class AddDaysToClientUseCase
{
    private readonly IClientRepository _repo;
    private readonly IUnitOfWork _uow;

    public AddDaysToClientUseCase(IClientRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Result> ExecuteAsync(Guid clientId, int days)
    {
        var client = await _repo.GetByIdAsync(clientId);

        if (client == null)
            return Result.Fail("Client not found", ErrorCode.ClientNotFound);

        client.AddDaysToExpiration(days);

        await _uow.CommitAsync();

        return Result.Ok();
    }
}