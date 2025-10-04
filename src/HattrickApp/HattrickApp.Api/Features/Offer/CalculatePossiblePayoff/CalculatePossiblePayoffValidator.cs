using FluentValidation;

namespace HattrickApp.Api.Features.Offer.CalculatePossiblePayoff;

public class CalculatePossiblePayoffValidator : AbstractValidator<CalculatePossiblePayoffHandler.Command>
{
    public CalculatePossiblePayoffValidator()
    {
        RuleFor(x => x.SelectedQuotas)
            .NotEmpty()
            .ForEach(q => q.GreaterThan(0));

        RuleFor(x => x.BetAmount)
            .GreaterThan(0);
    }
}
