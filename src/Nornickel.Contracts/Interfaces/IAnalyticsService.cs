using Nornickel.Common;
using Nornickel.Contracts.Dtos.Analytic;

namespace Nornickel.Contracts.Interfaces;

public interface IAnalyticsService
{
    Task<ServiceResult<ChartDataResponseDto>> GetSalaryDistributionAsync();
    Task<ServiceResult<ChartDataResponseDto>> GetAgeDistributionAsync();
    Task<ServiceResult<ChartDataResponseDto>> GetSalaryByExperienceAsync();
}
