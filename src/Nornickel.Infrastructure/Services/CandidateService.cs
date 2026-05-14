using Microsoft.EntityFrameworkCore;
using Nornickel.Common;
using Nornickel.Contracts.Dtos.Candidate;
using Nornickel.Contracts.Enums;
using Nornickel.Contracts.Interfaces;
using Nornickel.Database.Contexts;
using Nornickel.Database.DataModels;
using Nornickel.Database.Enums;

namespace Nornickel.Infrastructure.Services;

internal class CandidateService(
    IDbContextFactory<HrSystemDbContext> dbContextFactory
    ) : ICandidateService
{
    private readonly IDbContextFactory<HrSystemDbContext> _dbContextFactory = dbContextFactory;

    public async Task<ServiceResult<List<CandidateResponseDto>>> GetAllAsync()
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var result = await context.Set<CandidateDataModel>()
            .Select(c => new CandidateResponseDto
            {
                Id = c.Id,
                FullName = c.FullName,
                DateOfBirth = c.DateOfBirth,
                ExpectedSalary = c.ExpectedSalary,
                ExperienceYears = c.ExperienceYears,
                TechStack = c.TechStack,
                ContactInfo = c.ContactInfo,
                Status = (CandidateStatusDto)c.Status,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();

        return ServiceResult<List<CandidateResponseDto>>.Ok(result);
    }

    public async Task<ServiceResult<CandidateResponseDto>> CreateAsync(CandidateCreateRequestDto request)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var candidate = new CandidateDataModel
        {
            FullName = request.FullName,
            DateOfBirth = request.DateOfBirth,
            ExpectedSalary = request.ExpectedSalary,
            ExperienceYears = request.ExperienceYears,
            TechStack = request.TechStack,
            ContactInfo = request.ContactInfo,
            Status = CandidateStatus.New,
            CreatedAt = DateTime.UtcNow
        };

        context.Set<CandidateDataModel>().Add(candidate);
        await context.SaveChangesAsync();

        var result = new CandidateResponseDto
        {
            Id = candidate.Id,
            FullName = candidate.FullName,
            DateOfBirth = candidate.DateOfBirth,
            ExpectedSalary = candidate.ExpectedSalary,
            ExperienceYears = candidate.ExperienceYears,
            TechStack = candidate.TechStack,
            ContactInfo = candidate.ContactInfo,
            Status = (CandidateStatusDto)candidate.Status,
            CreatedAt = candidate.CreatedAt
        };

        return ServiceResult<CandidateResponseDto>.Ok(result);
    }
}
