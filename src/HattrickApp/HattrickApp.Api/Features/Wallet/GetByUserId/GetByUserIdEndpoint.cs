using Carter;
using HattrickApp.Api.Common.ResultPattern;
using HattrickApp.Api.Constants;
using MediatR;

namespace HattrickApp.Api.Features.Wallet.GetByUserId;

public class GetByUserIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapGet(EndpointRoutes.GetByUserId, async (Guid userId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                GetByUserIdHandler.Query query = new() { UserId = userId };
                Result<GetByUserIdResponse> result = await sender.Send(query, cancellationToken);
                return result.HandleResult(result);
            })
            .WithTags(EndpointTags.Wallet);
}
