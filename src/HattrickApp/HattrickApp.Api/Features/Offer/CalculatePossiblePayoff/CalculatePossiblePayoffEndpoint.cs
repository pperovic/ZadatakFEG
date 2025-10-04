using Carter;
using HattrickApp.Api.Common.ResultPattern;
using HattrickApp.Api.Constants;
using Mapster;
using MediatR;

namespace HattrickApp.Api.Features.Offer.CalculatePossiblePayoff;

/// <summary>
/// Can be used to show possible payoff on ui before creating a ticket
/// </summary>
public class CalculatePossiblePayoffEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost(EndpointRoutes.CalculatePossiblePayoff, async (CalculatePossiblePayoffRequest request,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                CalculatePossiblePayoffHandler.Command command = request.Adapt<CalculatePossiblePayoffHandler.Command>();
                Result<CalculatePossiblePayoffResponse> result = await sender.Send(command, cancellationToken);
                return result.HandleResult(result);
            })
            .WithTags(EndpointTags.Offer);
}
