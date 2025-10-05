namespace HattrickApp.Api.Features.Ticket.Create;

public record TicketSelectionResponse
{
    public required Guid OfferId { get; init; }
    public required string TipCode { get; init; } 
    public required decimal Quota { get; init; }
}
