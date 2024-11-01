using Bogus;

namespace CodeFlix.Catalog.SharedTests.Base;

public abstract class BaseFixture
{
    protected Faker Faker { get; set; }

    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
    }

}
