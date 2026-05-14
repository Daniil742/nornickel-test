using Nornickel.Common;
using Nornickel.Contracts.Dtos.Candidate;

namespace Nornickel.Contracts.Interfaces;

public interface ICandidateService
{
    Task<ServiceResult<List<CandidateResponseDto>>> GetAllAsync();
    Task<ServiceResult<CandidateResponseDto>> CreateAsync(CandidateCreateRequestDto request);
}
