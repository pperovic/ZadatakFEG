using HattrickApp.Api.Common;
using HattrickApp.Api.Enums;

namespace HattrickApp.Api.Entities;

public class Offer : EntityBase<Guid>
{
    public required string FirstCompetitor { get; set; } 
    public required string SecondCompetitor { get; set; }

    public DateTimeOffset StartTime { get; set; }
    public bool IsTopOffer { get; set; }
    public SportType SportType { get; set; }
    
    public ICollection<OfferTip> Tips { get; set; } = new List<OfferTip>();
}
