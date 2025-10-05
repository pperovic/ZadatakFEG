using HattrickApp.Api.Common.Dtos;

namespace HattrickApp.Api.Services.BetCalculationService;

public interface IBetCalculationService
{
    BetCalculationResultDto Calculate(decimal betAmount, IEnumerable<decimal> quotas);
}
