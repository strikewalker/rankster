namespace Rankster.Common.Models;

public record RanksterModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<RankingItem> Items { get; set; }
    public decimal CostUsd { get; set; }
}
