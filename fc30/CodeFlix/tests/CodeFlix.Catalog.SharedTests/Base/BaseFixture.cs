using Bogus;

namespace CodeFlix.Catalog.SharedTests.Base;

public abstract class BaseFixture
{
    public Faker Faker { get; set; }

    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
    }

}
