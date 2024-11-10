using CodeFlix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using SharedBase = CodeFlix.Catalog.SharedTests.Base;

namespace CodeFlix.Catalog.EndToEndTests.Base;


public class BaseFixture : SharedBase.BaseFixture
{
    public ApiClient ApiClient { get; set; }
    public CustomWebApplicationFactory<ProgramMarker> WebAppFactory { get; set; }
    public HttpClient HttpClient { get; set; }

    public BaseFixture() : base()
    {
        WebAppFactory = new CustomWebApplicationFactory<ProgramMarker>();
        HttpClient = WebAppFactory.CreateClient();
        ApiClient = new ApiClient(HttpClient);
    }

    public CodeFlixCatalogDbContext CreateDbContext()
    {
        var dbContext = new CodeFlixCatalogDbContext(
            new DbContextOptionsBuilder<CodeFlixCatalogDbContext>()
                .UseInMemoryDatabase(CustomWebApplicationFactory.InMemoryDatabaseName)
                .Options
        );
        return dbContext;
    }
}
