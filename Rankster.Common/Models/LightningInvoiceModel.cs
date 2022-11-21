namespace Rankster.Common.Models;

public class LightningInvoiceModel
{
    public Guid VoteId { get; set; }
    public string LnInvoiceId { get; set; }
    public long ExpirationInSeconds { get; set; }
    public decimal CostUsd { get; set; }
}
