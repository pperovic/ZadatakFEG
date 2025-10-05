using HattrickApp.Api.Common;

namespace HattrickApp.Api.Entities;

public class Ticket :EntityBase<Guid>
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
    public decimal BetAmount { get; set; }
    public decimal ManipulativeCost { get; set; }
    public decimal TotalQuota { get; set; }
    public decimal PossibleWinBeforeTax { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal PossibleWinAfterTax { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    public ICollection<TicketSelection> TicketSelections { get; set; } = new List<TicketSelection>();
}
