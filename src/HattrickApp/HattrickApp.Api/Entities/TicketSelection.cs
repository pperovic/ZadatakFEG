using HattrickApp.Api.Common;

namespace HattrickApp.Api.Entities;

public class TicketSelection : EntityBase<Guid>
{
    public Guid TicketId { get; set; }
    public Ticket? Ticket { get; set; } 
    
    public Guid OfferId { get; set; }
    public Offer? Offer { get; set; }
    public required string ChosenTipCode { get; set; }
    public decimal ChosenTipCodeQuota { get; set; }
}
