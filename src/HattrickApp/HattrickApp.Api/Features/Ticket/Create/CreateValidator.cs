using FluentValidation;

namespace HattrickApp.Api.Features.Ticket.Create;

public class CreateValidator : AbstractValidator<CreateHandler.Command>
{
    public CreateValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.BetAmount)
            .GreaterThan(0);

        RuleFor(x => x.Selections)
            .NotEmpty();
        
        RuleForEach(x => x.Selections).ChildRules(selection =>
        {
            selection.RuleFor(s => s.OfferId)
                .NotEmpty();

            selection.RuleFor(s => s.TipCode)
                .NotEmpty();
        });
    }
}
