using Nornickel.Contracts.Dtos.Analytic;
using Nornickel.Contracts.Interfaces;

namespace Nornickel.Api.Endpoints;

public static partial class Endpoints
{
    public static IEndpointRouteBuilder RegisterAnalyticEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var mapGroup = endpoints
            .MapGroup("/api/{VersionV10}/analytics")
            .WithTags("Аналитика")
            .RequireAuthorization();

        mapGroup
            .MapGet("/salary-distribution", async (IAnalyticsService analyticsService) =>
            {
                var result = await analyticsService.GetSalaryDistributionAsync();

                if (result.IsFailed)
                    return Results.Problem(result.Details);

                return Results.Ok(result.Value);
            })
            .WithSummary("Распределение по зарплатам")
            .WithDescription("Возвращает данные для барчарта по диапазонам желаемых зарплат кандидатов.")
            .Produces<ChartDataResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        mapGroup
            .MapGet("/age-distribution", async (IAnalyticsService analyticsService) =>
            {
                var result = await analyticsService.GetAgeDistributionAsync();

                if (result.IsFailed)
                    return Results.Problem(result.Details);

                return Results.Ok(result.Value);
            })
            .WithSummary("Распределение по возрасту")
            .WithDescription("Возвращает данные для барчарта по возрастным группам кандидатов.")
            .Produces<ChartDataResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        mapGroup
            .MapGet("/salary-by-experience", async (IAnalyticsService analyticsService) =>
            {
                var result = await analyticsService.GetSalaryByExperienceAsync();

                if (result.IsFailed)
                    return Results.Problem(result.Details);

                return Results.Ok(result.Value);
            })
            .WithSummary("Зависимость ЗП от опыта")
            .WithDescription("Возвращает данные для графика корреляции средней желаемой зарплаты от опыта работы.")
            .Produces<ChartDataResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        return endpoints;
    }
}
