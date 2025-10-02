namespace HattrickApp.Api.Features.Wallet.Deposit;

public record DepositRequest
{
    public required Guid WalletId { get; init; }
    public required decimal Amount { get; init; }
}
