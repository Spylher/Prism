using Prism.Application.Common;
using Prism.Domain.Entities;

namespace Prism.Application.UseCases.Auth;

public class ValidateSubscriptionUseCase
{
    public Result Execute(Client client)
    {
        if (!client.IsActive)
            return Result.Fail("Account inactive", ErrorCode.Forbidden);

        if (client.IsExpired())
            return Result.Fail("Subscription expired", ErrorCode.Forbidden);

        return Result.Ok();
    }
}