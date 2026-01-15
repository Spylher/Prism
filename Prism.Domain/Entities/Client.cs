using Prism.Domain.Exceptions;

namespace Prism.Domain.Entities;

public class Client
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected Client() { }

    public Client(string firstName, string lastName)
    {
        Id = Guid.NewGuid();
        UpdateName(firstName, lastName);
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required.");

        FirstName = firstName;
        LastName = lastName;
    }

    public void SetActiveStatus(bool isActive)
    {
        if (isActive)
            Activate();
        else
            Deactivate();
    }

    private void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;
    }

    private void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
    }
}
