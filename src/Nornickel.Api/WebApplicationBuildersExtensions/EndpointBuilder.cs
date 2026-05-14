using Nornickel.Api.Endpoints;

namespace Nornickel.Api.WebApplicationBuildersExtensions;

public static class EndpointBuilder
{
    public static void UseEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.RegisterAuthenticationEndpoints();
        endpoints.RegisterCandidateEndpoints();
        endpoints.RegisterAnalyticEndpoints();
    }
}
