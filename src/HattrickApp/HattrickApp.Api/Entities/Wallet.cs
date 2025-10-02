using HattrickApp.Api.Common;

namespace HattrickApp.Api.Entities;

public class Wallet : EntityBase<Guid>
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public decimal Balance { get; set; }

    public ICollection<WalletTransaction>? WalletTransactions { get; set; }
}
