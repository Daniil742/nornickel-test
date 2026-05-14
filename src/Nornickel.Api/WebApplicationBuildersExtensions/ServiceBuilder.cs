using Nornickel.Infrastructure;

namespace Nornickel.Api.WebApplicationBuildersExtensions;

public static class ServiceBuilder
{
    public static IServiceCollection AddServiceRegister(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.ConfigureServices(configuration);

        return serviceCollection;
    }
}
