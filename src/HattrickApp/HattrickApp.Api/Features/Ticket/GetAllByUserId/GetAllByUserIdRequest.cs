namespace HattrickApp.Api.Features.Ticket.GetAllByUserId;

public record GetAllByUserIdRequest
{
    public required Guid UserId { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
