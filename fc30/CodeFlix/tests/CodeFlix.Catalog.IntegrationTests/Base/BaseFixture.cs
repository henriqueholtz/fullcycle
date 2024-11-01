using CodeFlix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using SharedBase = CodeFlix.Catalog.SharedTests.Base;

namespace CodeFlix.Catalog.IntegrationTests.Base;

public class BaseFixture : SharedBase.BaseFixture
{
    public CodeFlixCatalogDbContext CreateDbContext(bool preserveData = false, bool randomDatabaseName = false)
    {
        string databaseName = "CodeFlix.Catalog.IntegrationTests";
        if (randomDatabaseName)
            databaseName += Guid.NewGuid().ToString();

        var dbContext = new CodeFlixCatalogDbContext(
            new DbContextOptionsBuilder<CodeFlixCatalogDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options
        );
        if (!preserveData)
            dbContext.Database.EnsureDeleted();
        return dbContext;
    }
}
