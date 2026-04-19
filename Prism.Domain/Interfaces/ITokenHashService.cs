namespace Prism.Domain.Interfaces;

public interface ITokenHashService
{
    string Compute(string input);
}