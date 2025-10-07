namespace HattrickApp.Api.Features.Ticket.GetAllByUserId;

public record GetAllByUserIdResponse
{
    public required Guid Id { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required decimal BetAmount { get; init; }
    public required decimal ManipulativeCost { get; init; }
    public required decimal TotalQuota { get; init; }
    public required decimal PossibleWinBeforeTax { get; init; }
    public required decimal TaxAmount { get; init; }
    public required decimal PossibleWinAfterTax { get; init; }
    public required List<TicketSelectionDto> TicketSelections { get; init; } = [];
}
