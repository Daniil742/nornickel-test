using Nornickel.Contracts.Enums;

namespace Nornickel.Contracts.Dtos.Candidate;

public class CandidateResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public decimal ExpectedSalary { get; set; }
    public int ExperienceYears { get; set; }
    public string TechStack { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;
    public CandidateStatusDto Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
