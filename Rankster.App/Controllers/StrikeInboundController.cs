using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Rankster.App.Hubs;

namespace Rankster.App.Controllers;
[ApiController]
[Route("api/[controller]")]
public class StrikeInboundController : ControllerBase
{
    private readonly ILogger<StrikeInboundController> _logger;
    private readonly IInvoiceService _invoiceService;
    private readonly IRanksterService _ranksterService;
    private readonly IHubContext<NotificationHub> _hub;

    public StrikeInboundController(ILogger<StrikeInboundController> logger, IInvoiceService invoiceService, IHubContext<NotificationHub> hub, IRanksterService ranksterService)
    {
        _logger = logger;
        _invoiceService = invoiceService;
        _ranksterService = ranksterService;
        _hub = hub;
    }

    [HttpPost]
    public async Task Post([FromBody] InvoiceWebhookEvent body)
    {
        var result = await _invoiceService.HandlePaidInvoice(body.Data.EntityId);
        if (!string.IsNullOrWhiteSpace(result?.code))
        {
            await _hub.Clients.Group(result.voteId.ToString()).SendAsync("VotePaid", result.voteId);
            var rankster = await _ranksterService.GetRankster(result.code);
            await _hub.Clients.Group(result.code).SendAsync("ItemsUpdated", rankster.Items);
        }
    }
}
public class InvoiceWebhookEvent
{
    public InvoiceRef Data { get; set; }
}
public class InvoiceRef
{
    public Guid EntityId { get; set; }
}