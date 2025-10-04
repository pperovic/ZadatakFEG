using HattrickApp.Api.Enums;

namespace HattrickApp.Api.Features.Offer.GetAll;

public class GetAllResponse
{
    public required Guid Id { get; init; }
    public required string FirstCompetitor { get; init; }
    public required string SecondCompetitor { get; init; }
    public required SportType SportType { get; init; }
    public required DateTimeOffset StartTime { get; init; }
    public required bool IsTopOffer { get; init; }
    public required List<GetAllOfferTipDto> Tips { get; init; } = [];
}
