namespace HattrickApp.Api.Features.Offer.CalculatePossiblePayoff;

public record CalculatePossiblePayoffResponse
{
    public decimal TotalQuota { get; init; }
    public decimal FullBetPlaced { get; init; }
    public decimal ManipulativeCost { get; init; }
    public decimal BetPlaced => FullBetPlaced - ManipulativeCost;
    public decimal PossibleWin { get; init; }
    public decimal TaxAmount { get; init; } 
    public decimal PossiblePayoff { get; init; }
}
