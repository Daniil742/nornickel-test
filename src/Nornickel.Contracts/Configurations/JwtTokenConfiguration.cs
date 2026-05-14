using System.ComponentModel.DataAnnotations;

namespace Nornickel.Contracts.Configurations;

public class JwtTokenConfiguration
{
    public const string ConfigurationSectionName = "Jwt";

    [Required(ErrorMessage = $"{nameof(Key)} обязателен для заполнения.")]
    public string Key { get; set; }

    [Required(ErrorMessage = $"{nameof(Issuer)} обязателен для заполнения.")]
    public string Issuer { get; set; }

    [Required(ErrorMessage = $"{nameof(Audience)} обязателен для заполнения.")]
    public string Audience { get; set; }

    [Required(ErrorMessage = $"{nameof(ExpiresInMinutes)} обязателен для заполнения.")]
    public int ExpiresInMinutes { get; set; }
}
