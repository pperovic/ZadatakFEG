using HattrickApp.Api.Common;
using HattrickApp.Api.Enums;

namespace HattrickApp.Api.Entities;

public class WalletTransaction : EntityBase<Guid>
{
    public Guid WalletId { get; set; }
    public Wallet? Wallet { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public TransactionType TransactionType { get; set; }
}
