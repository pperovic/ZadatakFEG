using HattrickApp.Api.Common.Dtos;
using HattrickApp.Api.Constants;

namespace HattrickApp.Api.Services.BetCalculationService;

public class BetCalculationService : IBetCalculationService
{
    public BetCalculationResultDto CalculateBetInfo(decimal betAmount, IEnumerable<decimal> quotas)
    {
        // starting from 1.0, multiply each quota with current total
        decimal totalQuota = quotas.Aggregate(1.0m, (currentTotal, quota) => currentTotal * quota);

        decimal manipulativeCost = betAmount * ApiConstants.ManipulativeCost;
        decimal betAfterCost = betAmount - manipulativeCost;

        decimal possibleWinBeforeTax = betAfterCost * totalQuota;
        decimal taxAmount = possibleWinBeforeTax * ApiConstants.TaxCost;
        decimal possibleWinAfterTax = possibleWinBeforeTax - taxAmount;
        
        return new BetCalculationResultDto
        {
            TotalQuota = Math.Round(totalQuota, 2),
            FullBetPlaced = Math.Round(betAmount, 2),
            ManipulativeCost = Math.Round(manipulativeCost, 2),
            PossibleWinBeforeTax = Math.Round(possibleWinBeforeTax, 2),
            TaxAmount = Math.Round(taxAmount, 2),
            PossibleWinAfterTax = Math.Round(possibleWinAfterTax, 2)
        };
    }
}
