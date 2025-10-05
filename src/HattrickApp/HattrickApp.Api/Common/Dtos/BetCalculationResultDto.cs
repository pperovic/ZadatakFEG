namespace HattrickApp.Api.Common.Dtos;

public record BetCalculationResultDto
{
    public required decimal TotalQuota { get; init; }
    public required decimal FullBetPlaced { get; init; }
    public required decimal ManipulativeCost { get; init; }
    public required decimal PossibleWinBeforeTax { get; init; }
    public required decimal TaxAmount { get; init; }
    public required decimal PossibleWinAfterTax { get; init; }
}
