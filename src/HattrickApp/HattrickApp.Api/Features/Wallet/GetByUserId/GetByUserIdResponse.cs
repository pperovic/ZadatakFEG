namespace HattrickApp.Api.Features.Wallet.GetByUserId;

public record GetByUserIdResponse
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }
}
