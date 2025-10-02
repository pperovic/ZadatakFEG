using FluentValidation;

namespace HattrickApp.Api.Features.Wallet.Deposit;

public class DepositValidator : AbstractValidator<DepositHandler.Command>
{
    public DepositValidator()
    {
        RuleFor(x => x.WalletId)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .PrecisionScale(18, 2, ignoreTrailingZeros: true);
    }
}
