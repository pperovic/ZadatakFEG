using Carter;
using HattrickApp.Api.Common.ResultPattern;
using HattrickApp.Api.Constants;
using Mapster;
using MediatR;

namespace HattrickApp.Api.Features.Wallet.Deposit;

public class DepositEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) => 
        app.MapPost(EndpointRoutes.Deposit, async (DepositRequest depositRequest,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            DepositHandler.Command command = depositRequest.Adapt<DepositHandler.Command>();
            Result<DepositResponse> result = await sender.Send(command, cancellationToken);
            return result.HandleResult(result);
        })
        .WithTags(EndpointTags.Wallet);
         
}
