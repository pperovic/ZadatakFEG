using HattrickApp.Api.Common;

namespace HattrickApp.Api.Entities;

public class OfferTip : EntityBase<Guid>
{
    public Guid OfferId { get; set; }
    public Offer? Offer  { get; set; }

    public required string TipCode { get; set; }
    public decimal? Quota { get; set; }
}
