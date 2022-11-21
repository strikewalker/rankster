using Rankster.Common.Models;

namespace Rankster.Common.Logic;

public class InvoiceService : IInvoiceService
{
    private readonly IStrikeFacade _strikeFacade;
    private readonly AppDbContext _db;

    public InvoiceService(IStrikeFacade strikeFacade, AppDbContext db)
    {
        _strikeFacade = strikeFacade;
        _db = db;
    }

    public async Task<LightningInvoiceModel> CreateLnInvoice(Guid rankItemId, Guid sessionKey, VoteType voteType)
    {
        var dbVote = _db.Votes.Where(x => x.Type == voteType && x.SessionKey == sessionKey && x.RankItemId == rankItemId && x.Paid == null).Include(r => r.RankItem).ThenInclude(r => r.Rankster).FirstOrDefault();
        DbRankster rankster;
        if (dbVote == null)
        {
            var rankItem = _db.RankItems.Where(r => r.Id == rankItemId).Include(r => r.Rankster).First();
            rankster = rankItem.Rankster!;
            var voteId = Guid.NewGuid();
            var invoiceId = await _strikeFacade.CreateInvoice(rankster.StrikeUsername, rankster.CostUsd, rankster.Currency,
                $"{rankItem.Name} - Vote {(voteType is VoteType.Up ? "Up" : "Down")} - {sessionKey}", voteId);
            dbVote = new DbVote { Id = voteId, RankItemId = rankItemId, StrikeInvoiceId = invoiceId, SessionKey = sessionKey, Type = voteType };
            _db.Add(dbVote);
            await _db.SaveChangesAsync();
        }
        else {
            rankster = dbVote.RankItem.Rankster;
        }
        var lnInvoice = await _strikeFacade.CreateLnInvoice(dbVote.StrikeInvoiceId);
        return new() {VoteId = dbVote.Id, ExpirationInSeconds = lnInvoice.ExpirationInSec, LnInvoiceId = lnInvoice.LnInvoice, CostUsd = rankster.CostUsd };
    }
    public async Task<PaidInvoiceInfo?> HandlePaidInvoice(Guid strikeInvoiceId)
    {
        var dbVote = _db.Votes.Include(v => v.RankItem).ThenInclude(v => v.Rankster).Where(x => x.StrikeInvoiceId == strikeInvoiceId && x.Paid == null).FirstOrDefault();
        if (dbVote == null) {
            return null;
        }
        dbVote.Paid = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return new(dbVote.Id, dbVote.RankItem.Rankster.Code);
    }
}

public record PaidInvoiceInfo(Guid voteId, string code);

public interface IInvoiceService
{
    Task<LightningInvoiceModel> CreateLnInvoice(Guid rankItemId, Guid sessionKey, VoteType voteType);
    Task<PaidInvoiceInfo?> HandlePaidInvoice(Guid strikeInvoiceId);
}
