﻿using Rankster.Data.External.Strike;
using Rankster.Data.External.Strike.Models;

namespace Rankster.Data.External;

public class StrikeFacade : IStrikeFacade
{
    private readonly IStrikeClient _strikeClient;

    public StrikeFacade(IStrikeClient strikeClient)
    {
        _strikeClient = strikeClient;
    }

    public async Task<Guid> CreateInvoice(string strikeUsername, decimal amountUSD, string? currency, string description, Guid emailId)
    {
        var result = await _strikeClient.HandleAsync(strikeUsername, new()
        {
            Amount = new CurrencyAmount
            {
                Amount = amountUSD.ToString("f2"),
                Currency = Enum.Parse<CurrencyAmountCurrency>(currency ?? "USD")
            },
            Description = description,
            CorrelationId = emailId.ToString()
        });
        return result.InvoiceId;
    }

    public async Task<bool> InvoiceIsPaid(Guid strikeInvoiceId)
    {
        var invoice = await _strikeClient.InvoicesGET2Async(strikeInvoiceId);
        return invoice.State == Strike.Models.InvoiceState.PAID;
    }

    public Task<InvoiceQuote> CreateLnInvoice(Guid strikeInvoiceId)
    {
        return _strikeClient.QuoteAsync(strikeInvoiceId);
    }

    public Task<AccountProfile> GetProfile(string strikeUsername) { 
        return _strikeClient.Profile2Async(strikeUsername);
    }
}

public interface IStrikeFacade
{
    Task<Guid> CreateInvoice(string strikeUsername, decimal amountUSD, string? currency, string description, Guid emailId);
    Task<InvoiceQuote> CreateLnInvoice(Guid strikeInvoiceId);
    Task<bool> InvoiceIsPaid(Guid strikeInvoiceId);
    Task<AccountProfile> GetProfile(string strikeUsername);
}