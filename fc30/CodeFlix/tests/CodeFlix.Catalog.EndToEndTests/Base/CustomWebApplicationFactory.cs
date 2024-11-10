using CodeFlix.Catalog.Infra.Data.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeFlix.Catalog.EndToEndTests.Base;

public class CustomWebApplicationFactory
{
    public const string InMemoryDatabaseName = "end2end-db";
}

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbOptions = services.FirstOrDefault(x => x.ServiceType == typeof(DbContextOptions<CodeFlixCatalogDbContext>));
            if (dbOptions != null)
                services.Remove(dbOptions);

            services.AddDbContext<CodeFlixCatalogDbContext>(options =>
            {
                options.UseInMemoryDatabase(CustomWebApplicationFactory.InMemoryDatabaseName);
            });
        });
        base.ConfigureWebHost(builder);
    }
}
