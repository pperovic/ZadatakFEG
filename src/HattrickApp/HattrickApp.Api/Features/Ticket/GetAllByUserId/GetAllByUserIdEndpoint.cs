using Carter;
using HattrickApp.Api.Common.Dtos;
using HattrickApp.Api.Common.ResultPattern;
using HattrickApp.Api.Constants;
using Mapster;
using MediatR;

namespace HattrickApp.Api.Features.Ticket.GetAllByUserId;

public class GetAllByUserIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) => 
        app.MapPost(EndpointRoutes.GetAllByUserId, async (GetAllByUserIdRequest getAllRequest,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                GetAllByUserIdHandler.Query query = getAllRequest.Adapt<GetAllByUserIdHandler.Query>();
                Result<PagedResultDto<GetAllByUserIdResponse>> result = await sender.Send(query, cancellationToken);
                return result.HandleResult(result);
            })
            .WithTags(EndpointTags.Ticket);
}
