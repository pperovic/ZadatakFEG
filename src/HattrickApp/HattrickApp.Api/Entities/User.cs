using HattrickApp.Api.Common;

namespace HattrickApp.Api.Entities;

// Keep simple for this example, but usually would inherit from IdentityUser<Guid>
public class User : EntityBase<Guid>
{
    public required string UserName { get; set; }
    public Wallet? Wallet { get; set; }
}
