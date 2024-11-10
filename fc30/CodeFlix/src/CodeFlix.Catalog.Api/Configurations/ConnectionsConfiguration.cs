using CodeFlix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CodeFlix.Catalog.Api.Configurations;

public static class ConnectionsConfiguration
{
    public static IServiceCollection AddAllConnections(this IServiceCollection services)
    {
        services.AddDbConnection();
        return services;
    }

    private static IServiceCollection AddDbConnection(this IServiceCollection services)
    {
        services.AddDbContext<CodeFlixCatalogDbContext>(options =>
        {
            options.UseInMemoryDatabase("InMemory-CodeFlix-Db");
        });
        return services;
    }
}
