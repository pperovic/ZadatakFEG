namespace HattrickApp.Api.Features.Offer.GetAll;

public class GetAllOfferTipDto
{
    public required string TipCode { get; init; } 
    public decimal? Quota { get; init; }
}
