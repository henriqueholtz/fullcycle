using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlix.Catalog.Domain.Entity;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public Category(string name, string description)
    {
        Name = name;
        Description = description;
        Id = Guid.NewGuid();
        IsActive = true;
        CreatedAt = DateTime.Now;
    }
}
