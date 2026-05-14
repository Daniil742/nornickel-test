using Nornickel.Infrastructure;

namespace Nornickel.Api.WebApplicationBuildersExtensions;

public static class DbContextBuilder
{
    public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.ConfigureAppDbContext(configuration);

        return serviceCollection;
    }

    public static void DbMigrateHrSystem(this WebApplication app)
    {
        app.ConfigureDbMigrate();
    }
}
