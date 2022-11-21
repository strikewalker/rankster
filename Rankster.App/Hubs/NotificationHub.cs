using Microsoft.AspNetCore.SignalR;
using Rankster.Data.Enums;

namespace Rankster.App.Hubs;
public class NotificationHub : Hub
{
    private readonly ILogger<NotificationHub> _logger;
    private readonly IRanksterService _ranksterService;
    private readonly IInvoiceService _invoiceService;

    public NotificationHub(ILogger<NotificationHub> logger, IRanksterService rankerService, IInvoiceService invoiceService)
    {
        _logger = logger;
        _ranksterService = rankerService;
        _invoiceService = invoiceService;
    }
    public async Task<RanksterModel> SubscribeToRankster(string code)
    {
        var rankster = await _ranksterService.GetRankster(code);
        if (rankster == null)
            return null;
        await Groups.AddToGroupAsync(Context.ConnectionId, code);
        return rankster;
    }
    public async Task UnsubscribeFromRankster(string code)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, code);
    }
    public async Task<LightningInvoiceModel> SubscribeToVote(Guid rankItemId, Guid sessionKey, VoteType voteType)
    {
        var lnInvoice = await _invoiceService.CreateLnInvoice(rankItemId, sessionKey, voteType);
        await Groups.AddToGroupAsync(Context.ConnectionId, lnInvoice.VoteId.ToString());
        return lnInvoice;
    }
    public async Task UnsubscribeFromVote(Guid voteId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, voteId.ToString());
    }
}
