using Carter;
using HattrickApp.Api.Common.ResultPattern;
using HattrickApp.Api.Constants;
using Mapster;
using MediatR;

namespace HattrickApp.Api.Features.Ticket.Create;

public class CreateEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost(EndpointRoutes.Create, async (CreateRequest request, 
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                CreateHandler.Command command = request.Adapt<CreateHandler.Command>();
                Result<CreateResponse> result = await sender.Send(command, cancellationToken);
                return result.HandleResult(result);
            })
            .WithTags(EndpointTags.Ticket);
    
}
