using Carter;
using HattrickApp.Api.Common.Dtos;
using HattrickApp.Api.Common.ResultPattern;
using HattrickApp.Api.Constants;
using Mapster;
using MediatR;

namespace HattrickApp.Api.Features.Offer.GetAll;

/// <summary>
/// Gets all offers as paginated response
/// </summary>
public class GetAllEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) => 
        app.MapPost(EndpointRoutes.GetAll, async (GetAllRequest getAllRequest,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                GetAllHandler.Query query = getAllRequest.Adapt<GetAllHandler.Query>();
                Result<PagedResultDto<GetAllResponse>> result = await sender.Send(query, cancellationToken);
                return result.HandleResult(result);
            })
            .WithTags(EndpointTags.Offer);
}
