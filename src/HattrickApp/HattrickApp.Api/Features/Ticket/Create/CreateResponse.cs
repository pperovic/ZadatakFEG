namespace HattrickApp.Api.Features.Ticket.Create;

public record CreateResponse
{
    public required Guid TicketId { get; init; }
    public required decimal BetAmount { get; init; }
    public required decimal ManipulativeCost { get; init; }
    public required decimal TotalQuota { get; init; }
    public required decimal PossibleWinBeforeTax { get; init; }
    public required decimal TaxAmount { get; init; }
    public required decimal PossibleWinAfterTax { get; init; }
    public required List<TicketSelectionResponse> TicketSelections { get; init; } = [];
}
