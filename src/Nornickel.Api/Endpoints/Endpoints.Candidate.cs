using Microsoft.AspNetCore.Mvc;
using Nornickel.Contracts.Dtos.Candidate;
using Nornickel.Contracts.Interfaces;

namespace Nornickel.Api.Endpoints;

public static partial class Endpoints
{
    const string LOGGER_NAME = "CandidateEndpoints";

    public static IEndpointRouteBuilder RegisterCandidateEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var mapGroup = endpoints
            .MapGroup("/api/{VersionV10}/candidates")
            .WithTags("Кандидаты")
            .RequireAuthorization();

        mapGroup
            .MapGet("", async (ICandidateService candidateService) =>
            {
                var result = await candidateService.GetAllAsync();

                if (result.IsFailed)
                {
                    return result.Error == null
                        ? Results.Problem(result.Details)
                        : Results.ValidationProblem(result.Error, result.Details);
                }

                return Results.Ok(result.Value);
            })
            .WithSummary("Получить список кандидатов")
            .WithDescription("Возвращает всех добавленных в систему кандидатов.")
            .Produces<List<CandidateResponseDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        mapGroup
            .MapPost("", async (ICandidateService candidateService, ILoggerFactory loggerFactory, CandidateCreateRequestDto request) =>
            {
                try
                {
                    var result = await candidateService.CreateAsync(request);

                    if (result.IsFailed)
                    {
                        return result.Error == null
                            ? Results.Problem(result.Details)
                            : Results.ValidationProblem(result.Error, result.Details);
                    }

                    if (result.IsSuccess && result.Value == null)
                        return Results.NotFound(result.Details);

                    return Results.Ok(result.Value);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger(LOGGER_NAME);
                    logger.LogError(ex, "Ошибка при создании кандидата");
                    return Results.Problem("Внутренняя ошибка сервера при создании кандидата");
                }
            })
            .WithSummary("Добавить нового кандидата")
            .WithDescription("Создает карточку нового кандидата с указанием ФИО, ЗП, опыта и т.д.")
            .Produces<CandidateResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        return endpoints;
    }
}
