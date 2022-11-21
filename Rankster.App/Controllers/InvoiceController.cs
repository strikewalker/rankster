using Microsoft.AspNetCore.Mvc;
using Rankster.Data.Enums;

namespace Rankster.App.Controllers;
[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly ILogger<InvoiceController> _logger;
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(ILogger<InvoiceController> logger, IInvoiceService invoiceService)
    {
        _logger = logger;
        _invoiceService = invoiceService;
    }

    [HttpPost("/lninvoice")]
    public Task<LightningInvoiceModel> LnInvoice(Guid rankItemId, Guid sessionKey, VoteType voteType)
    {
        return _invoiceService.CreateLnInvoice(rankItemId, sessionKey, voteType);
    }
}
