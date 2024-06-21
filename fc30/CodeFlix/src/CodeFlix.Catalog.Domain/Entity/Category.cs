using CodeFlix.Catalog.Domain.Exceptions;

namespace CodeFlix.Catalog.Domain.Entity;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public Category(string name, string description, bool isActive = true)
    {
        Name = name;
        Description = description;
        Id = Guid.NewGuid();
        IsActive = isActive;
        CreatedAt = DateTime.Now;

        Validate();
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new EntityValidationException($"{nameof(Name)} should not be empty or null");

        if (Description is null)
            throw new EntityValidationException($"{nameof(Description)} should not be null");
    }
}
