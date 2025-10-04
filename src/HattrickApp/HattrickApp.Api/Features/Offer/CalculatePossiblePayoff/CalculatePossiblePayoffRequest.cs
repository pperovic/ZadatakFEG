namespace HattrickApp.Api.Features.Offer.CalculatePossiblePayoff;

public record CalculatePossiblePayoffRequest
{
    public required IReadOnlyCollection<decimal> SelectedQuotas { get; init; }
    public decimal BetAmount { get; set; } = 1.0m;
}
