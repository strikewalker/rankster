using Rankster.Common.Models;

namespace Rankster.Common.Logic;

public class RanksterService : IRanksterService
{
    private readonly AppDbContext _db;

    public RanksterService(AppDbContext db)
    {
        _db = db;
    }

    public Task<RanksterModel?> GetRankster(string code)
    {
        return _db.Ranksters.Where(r => r.Code == code && !r.Ended).Select(r => new RanksterModel
        {
            CostUsd = r.CostUsd,
            Description = r.Description,
            Name = r.Name,
            Items = r.RankItems.Select(r => new RankingItem
            {
                Id = r.Id,
                Description = r.Description,
                Name = r.Name,
                VotesDown = r.Votes.Count(v => v.Type == VoteType.Down && v.Paid.HasValue),
                VotesUp = r.Votes.Count(v => v.Type == VoteType.Up && v.Paid.HasValue),
            })
        }).FirstOrDefaultAsync();
    }
}

public interface IRanksterService
{
    Task<RanksterModel?> GetRankster(string code);
}
