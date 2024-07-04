﻿using CodeFlix.Catalog.Domain.Exceptions;
using CodeFlix.Catalog.Domain.SeedWork;

namespace CodeFlix.Catalog.Domain.Entity;

public class Category : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public Category(string name, string description, bool isActive = true)
        : base()
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.Now;

        Validate();
    }

    private void Validate()
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

    public void Activate()
    {
        IsActive = true;
        Validate();
    }

    public void Deactivate()
    {
        IsActive = false;
        Validate();
    }

    public void Update(string name, string? description = null)
    {
        Name = name;
        Description = description ?? Description;
        Validate();
    }
}
