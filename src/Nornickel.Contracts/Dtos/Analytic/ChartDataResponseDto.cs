namespace Nornickel.Contracts.Dtos.Analytic;

public class ChartDataResponseDto
{
    public List<string> Labels { get; set; } = new();
    public List<double> Values { get; set; } = new();
}
