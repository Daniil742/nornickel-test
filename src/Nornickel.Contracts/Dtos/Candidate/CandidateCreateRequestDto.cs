using System.ComponentModel.DataAnnotations;

namespace Nornickel.Contracts.Dtos.Candidate;

public class CandidateCreateRequestDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public decimal ExpectedSalary { get; set; }

    [Required]
    [Range(0, 50)]
    public int ExperienceYears { get; set; }

    [Required]
    public string TechStack { get; set; } = string.Empty;

    [Required]
    public string ContactInfo { get; set; } = string.Empty;
}
