using FluentValidation;
using FluentValidation.Results;
using HattrickApp.Api.Common.ResultPattern;
using HattrickApp.Api.Constants;
using MediatR;

namespace HattrickApp.Api.Features.Offer.CalculatePossiblePayoff;

public class CalculatePossiblePayoffHandler
{
    public record Command : CalculatePossiblePayoffRequest, IRequest<Result<CalculatePossiblePayoffResponse>>;

    internal sealed class Handler(IValidator<Command> validator)
        : IRequestHandler<Command, Result<CalculatePossiblePayoffResponse>>
    {
        public async Task<Result<CalculatePossiblePayoffResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            ValidationResult? validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result<CalculatePossiblePayoffResponse>.Failure(validationResult.Errors);
            }

            decimal totalQuota = request.SelectedQuotas.Aggregate(1.0m, (acc, q) => acc * q);

            decimal fullBet = request.BetAmount;
            decimal manipulativeCost = fullBet * ApiConstants.ManipulativeCost;
            decimal fullBetAfterManipulativeCost = fullBet - manipulativeCost;

            decimal possibleWinBeforeTax = fullBetAfterManipulativeCost * totalQuota;
            decimal taxAmount = possibleWinBeforeTax * ApiConstants.TaxCost;
            decimal possibleWinAfterTax = possibleWinBeforeTax - taxAmount;

            return await Task.FromResult(Result<CalculatePossiblePayoffResponse>.Success(new CalculatePossiblePayoffResponse
            {
                TotalQuota = Math.Round(totalQuota, 2),
                FullBetPlaced = Math.Round(fullBet, 2),
                ManipulativeCost = Math.Round(manipulativeCost, 2),
                PossibleWin = Math.Round(possibleWinBeforeTax, 2),
                TaxAmount = Math.Round(taxAmount, 2),
                PossiblePayoff = Math.Round(possibleWinAfterTax, 2)
            }));
        }
    }
}
