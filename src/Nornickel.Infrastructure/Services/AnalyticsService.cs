using Microsoft.EntityFrameworkCore;
using Nornickel.Common;
using Nornickel.Contracts.Dtos.Analytic;
using Nornickel.Contracts.Interfaces;
using Nornickel.Database.Contexts;
using Nornickel.Database.DataModels;

namespace Nornickel.Infrastructure.Services;

internal class AnalyticsService(
    IDbContextFactory<HrSystemDbContext> dbContextFactory
    ) : IAnalyticsService
{
    private readonly IDbContextFactory<HrSystemDbContext> _dbContextFactory = dbContextFactory;

    public async Task<ServiceResult<ChartDataResponseDto>> GetSalaryDistributionAsync()
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var candidates = await context.Set<CandidateDataModel>().ToListAsync();

        var ranges = new Dictionary<string, int>
        {
            {"До 100к", 0}, {"100к - 150к", 0}, {"150к - 200к", 0}, {"Более 200к", 0}
        };

        foreach (var candidate in candidates)
        {
            if (candidate.ExpectedSalary < 100000)
                ranges["До 100к"]++;
            else if (candidate.ExpectedSalary < 150000)
                ranges["100к - 150к"]++;
            else if (candidate.ExpectedSalary < 200000)
                ranges["150к - 200к"]++;
            else
                ranges["Более 200к"]++;
        }

        return ServiceResult<ChartDataResponseDto>.Ok(new ChartDataResponseDto
        {
            Labels = ranges.Keys.ToList(),
            Values = ranges.Values.Select(v => (double)v).ToList()
        });
    }

    public async Task<ServiceResult<ChartDataResponseDto>> GetAgeDistributionAsync()
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var candidates = await context.Set<CandidateDataModel>().ToListAsync();
        var today = DateTime.Today;

        var ranges = new Dictionary<string, int>
        {
            {"18-25", 0}, {"26-35", 0}, {"36-45", 0}, {"Старше 45", 0}
        };

        foreach (var candidate in candidates)
        {
            int age = today.Year - candidate.DateOfBirth.Year;
            if (candidate.DateOfBirth > today.AddYears(-age))
                age--;

            if (age <= 25)
                ranges["18-25"]++;
            else if (age <= 35)
                ranges["26-35"]++;
            else if (age <= 45)
                ranges["36-45"]++;
            else
                ranges["Старше 45"]++;
        }

        return ServiceResult<ChartDataResponseDto>.Ok(new ChartDataResponseDto
        {
            Labels = ranges.Keys.ToList(),
            Values = ranges.Values.Select(v => (double)v).ToList()
        });
    }

    public async Task<ServiceResult<ChartDataResponseDto>> GetSalaryByExperienceAsync()
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var candidates = await context.Set<CandidateDataModel>().ToListAsync();

        var grouped = candidates
            .GroupBy(c => c.ExperienceYears)
            .Select(g => new { Experience = $"{g.Key} лет", AvgSalary = g.Average(c => (double)c.ExpectedSalary) })
            .OrderBy(x => x.Experience)
            .ToList();

        return ServiceResult<ChartDataResponseDto>.Ok(new ChartDataResponseDto
        {
            Labels = grouped.Select(x => x.Experience).ToList(),
            Values = grouped.Select(x => x.AvgSalary).ToList()
        });
    }
}
