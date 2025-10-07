namespace HattrickApp.Api.Features.Wallet.GetByUserId;

public record GetByUserIdResponse
{
    public required Guid Id { get; init; }
    public required decimal Balance { get; init; }
}
