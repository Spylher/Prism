using Prism.Domain.Common;
using Prism.Domain.Exceptions;

namespace Prism.Domain.Entities;

public class Client
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public ClientLevel Level { get; set; } = ClientLevel.User;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    protected Client() { }

    public Client(string firstName, string lastName)
    {
        Id = Guid.NewGuid();
        UpdateName(firstName, lastName);
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddDays(3);
    }

    public bool IsExpired() => DateTime.UtcNow > ExpiresAt;

    public bool AddDaysToExpiration(int days)
    {
        //if (days == 0)
        //    throw new DomainException("Days cannot be zero.");

        if (ExpiresAt <= DateTime.UtcNow && days > 0)
        {
            ExpiresAt = DateTime.UtcNow.AddDays(days);
            return true;
        }


        ExpiresAt = ExpiresAt.AddDays(days);
        //if (this.ExpiresAt > DateTime.UtcNow.AddYears(5))
        //    return erro

        return true;
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
