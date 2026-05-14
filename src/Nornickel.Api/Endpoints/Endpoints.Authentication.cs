using Microsoft.AspNetCore.Mvc;
using Nornickel.Contracts.Dtos.Login;
using Nornickel.Contracts.Interfaces;

namespace Nornickel.Api.Endpoints;

public static partial class Endpoints
{
    const string VersionV10 = "v1.0";

    public static IEndpointRouteBuilder RegisterAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var mapGroup = endpoints
            .MapGroup("/api/{VersionV10}")
            .WithTags("Аутентификация");

        mapGroup
            .MapPost("/login", async (IAuthenticationService authenticationService, LoginRequestDto request) =>
            {
                var result = await authenticationService.LoginAsync(request);

                if (result.IsFailed)
                {
                    return result.Error == null
                        ? Results.Problem(result.Details)
                        : Results.ValidationProblem(result.Error, result.Details);
                }

                if (result.IsSuccess && result.Value == null)
                    return Results.NotFound(result.Details);

                return Results.Ok(result.Value);
            })
            .WithSummary("Вход на сайт")
            .WithDescription("Аутентификация пользователя по логину и паролю с выдачей результата операции.")
            .Produces<LoginResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .AllowAnonymous();

        return endpoints;
    }
}
