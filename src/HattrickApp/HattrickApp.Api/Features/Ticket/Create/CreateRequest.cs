namespace HattrickApp.Api.Features.Ticket.Create;

public record CreateRequest
{
    public required Guid UserId { get; init; }
    public required decimal BetAmount { get; init; } = 1.0m;
    public required List<CreateTicketSelectionDto> Selections { get; init; } = [];
}
