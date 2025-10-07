using HattrickApp.Api.Common.ResultPattern;
using HattrickApp.Api.Constants;
using HattrickApp.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HattrickApp.Api.Features.Wallet.GetByUserId;

public static class GetByUserIdHandler
{
    public record Query : IRequest<Result<GetByUserIdResponse>>
    {
        public required Guid UserId { get; init; }
    }

    internal sealed class Handler(
        HattrickAppDbContext dbContext) : IRequestHandler<Query, Result<GetByUserIdResponse>>
    {
        public async Task<Result<GetByUserIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            bool isUserExists = await dbContext.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken);
            if (!isUserExists)
            {
                return Result<GetByUserIdResponse>.Failure(ErrorMessage.NotFound(ApiConstants.User));
            }

            Entities.Wallet? userWallet = await dbContext.Wallets
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (userWallet is null)
            {
                return Result<GetByUserIdResponse>.Failure(ErrorMessage.NotFound(ApiConstants.Wallet));
            }

            return Result<GetByUserIdResponse>.Success(new GetByUserIdResponse
            {
                Id = userWallet.Id,
                Balance = userWallet.Balance
            });
        }
    }
}
