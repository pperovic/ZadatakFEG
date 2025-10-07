using FluentValidation;
using FluentValidation.Results;
using HattrickApp.Api.Common.Dtos;
using HattrickApp.Api.Common.ResultPattern;
using HattrickApp.Api.Services.BetCalculationService;
using MediatR;

namespace HattrickApp.Api.Features.Offer.CalculatePossiblePayoff;

public class CalculatePossiblePayoffHandler
{
    public record Command : CalculatePossiblePayoffRequest, IRequest<Result<CalculatePossiblePayoffResponse>>;

    internal sealed class Handler(IValidator<Command> validator,
        IBetCalculationService betCalculationService)
        : IRequestHandler<Command, Result<CalculatePossiblePayoffResponse>>
    {
        public async Task<Result<CalculatePossiblePayoffResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            ValidationResult? validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result<CalculatePossiblePayoffResponse>.Failure(validationResult.Errors);
            }

            BetCalculationResultDto result = betCalculationService.CalculateBetInfo(request.BetAmount, request.SelectedQuotas);
            
            return await Task.FromResult(Result<CalculatePossiblePayoffResponse>.Success(new CalculatePossiblePayoffResponse
            {
                TotalQuota = result.TotalQuota,
                FullBetPlaced = result.FullBetPlaced,
                ManipulativeCost = result.ManipulativeCost,
                PossibleWin = result.PossibleWinBeforeTax,
                TaxAmount = result.TaxAmount,
                PossiblePayoff = result.PossibleWinAfterTax
            }));
        }
    }
}
