namespace HattrickApp.Api.Features.Offer.GetAll;

public record GetAllRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
