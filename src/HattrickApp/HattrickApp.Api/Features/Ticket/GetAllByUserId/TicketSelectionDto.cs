namespace HattrickApp.Api.Features.Ticket.GetAllByUserId;

public class TicketSelectionDto
{
    public required Guid OfferId { get; init; }
    public required string ChosenTipCode { get; init; } 
    public required decimal ChosenTipCodeQuota { get; init; }
    public required string FirstCompetitor { get; init; }
    public required string SecondCompetitor { get; init; }
}
