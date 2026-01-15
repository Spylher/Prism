namespace Prism.Application.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync();
}
