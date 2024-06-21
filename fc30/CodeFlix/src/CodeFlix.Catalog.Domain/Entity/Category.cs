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

        if (Name.Length < 3)
            throw new EntityValidationException($"{nameof(Name)} should be at least 3 characters long");

        if (Name.Length > 255)
            throw new EntityValidationException($"{nameof(Name)} should be less or equal 255 characters long");

        if (Description is null)
            throw new EntityValidationException($"{nameof(Description)} should not be null");

        if (Description.Length > 10_000)
            throw new EntityValidationException($"{nameof(Description)} should be less or equal 10.000 characters long");
    }
}
