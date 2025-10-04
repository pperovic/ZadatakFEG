using FluentValidation;
using FluentValidation.Results;
using HattrickApp.Api.Common.ResultPattern;
using HattrickApp.Api.Constants;
using HattrickApp.Api.Entities;
using HattrickApp.Api.Enums;
using HattrickApp.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HattrickApp.Api.Features.Wallet.Deposit;

public static class DepositHandler
{
    public record Command : DepositRequest, IRequest<Result<DepositResponse>>;


    internal sealed class Handler(
        HattrickAppDbContext dbContext,
        IValidator<Command> validator) : IRequestHandler<Command, Result<DepositResponse>>
    {
        public async Task<Result<DepositResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            ValidationResult? validationResult = await validator.ValidateAsync(request, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                return Result<DepositResponse>.Failure(validationResult.Errors);
            }
            
            Entities.Wallet? existingUserWallet = await dbContext.Wallets
                .FirstOrDefaultAsync(w => w.Id == request.WalletId, cancellationToken);
            
            if (existingUserWallet is null)
            {
                return Result<DepositResponse>.Failure(ErrorMessage.NotFound(ApiConstants.Wallet));
            }
            
            existingUserWallet.Balance += request.Amount;
            var newWalletTransaction = new WalletTransaction
            {
                Id = Guid.NewGuid(),
                WalletId = existingUserWallet.Id,
                Amount = request.Amount,
                TransactionType = TransactionType.Deposit,
                CreatedAt = DateTimeOffset.UtcNow
            };
            
            await dbContext.WalletTransactions.AddAsync(newWalletTransaction, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            // response here would depend on how we do things on ui
            return Result<DepositResponse>.Success(new DepositResponse
            {
                NewWalletBalance = existingUserWallet.Balance
            });
            
        }
    }
}
