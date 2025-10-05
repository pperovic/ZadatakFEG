namespace HattrickApp.Api.Features.Ticket.Create;

public record CreateTicketSelectionDto
{
    public required Guid OfferId { get; init; }
    public required string TipCode { get; init; }
}
