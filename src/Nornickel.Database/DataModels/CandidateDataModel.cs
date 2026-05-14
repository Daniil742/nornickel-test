using Nornickel.Database.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nornickel.Database.DataModels;

public class CandidateDataModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "date")]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal ExpectedSalary { get; set; }

    [Required]
    [Range(0, 50)]
    public int ExperienceYears { get; set; }

    [Required]
    [MaxLength(200)]
    public string TechStack { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ContactInfo { get; set; } = string.Empty;

    [Required]
    public CandidateStatus Status { get; set; } = CandidateStatus.New;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
