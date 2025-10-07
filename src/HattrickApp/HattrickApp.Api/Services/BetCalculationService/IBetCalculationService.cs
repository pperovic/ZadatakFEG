using HattrickApp.Api.Common.Dtos;

namespace HattrickApp.Api.Services.BetCalculationService;

public interface IBetCalculationService
{
    BetCalculationResultDto CalculateBetInfo(decimal betAmount, IEnumerable<decimal> quotas);
}
