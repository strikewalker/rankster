namespace Rankster.Common.Models;

public record RankingItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int VotesUp { get; set; }
    public int VotesDown { get; set; }
}
