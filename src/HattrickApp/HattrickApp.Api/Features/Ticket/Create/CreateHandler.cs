using FluentValidation;
using FluentValidation.Results;
using HattrickApp.Api.Common.Dtos;
using HattrickApp.Api.Common.ResultPattern;
using HattrickApp.Api.Constants;
using HattrickApp.Api.Entities;
using HattrickApp.Api.Persistence;
using HattrickApp.Api.Services.BetCalculationService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HattrickApp.Api.Features.Ticket.Create;

public static  class CreateHandler
{
    public record Command : CreateRequest, IRequest<Result<CreateResponse>>;
    
    internal sealed class Handler(
        HattrickAppDbContext dbContext,
        IValidator<Command> validator,
        IBetCalculationService betCalculationService) : IRequestHandler<Command, Result<CreateResponse>>
    {
        public async Task<Result<CreateResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            ValidationResult? validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result<CreateResponse>.Failure(validationResult.Errors);
            }
            
            IReadOnlyList<Guid> offerIds = SelectOfferIdsFromRequestSelections(request.Selections);
            List<OfferTip> selectedTips = await GetOfferTipsForSelectedOfferIds(offerIds, cancellationToken);

            selectedTips = FilterTipsThatMatchRequestSelectionsOfferIdAndTipCode(request.Selections, selectedTips);

            if (selectedTips.Count != request.Selections.Count)
            {
                return Result<CreateResponse>.Failure(ErrorMessage.NotFound(ApiConstants.SelectedTips));
            }

            if (selectedTips.Any(t => t.Quota == null))
            {
                return Result<CreateResponse>.Failure(ErrorMessage.InvalidValues(ApiConstants.SelectedTips,
                    ApiConstants.Quota));
            }

            int topOffersCount = await GetTopOffersCount(offerIds, cancellationToken);

            if (topOffersCount > 1)
            {
                return Result<CreateResponse>.Failure(
                    ErrorMessage.MoreThanOneNotAllowed(ApiConstants.Ticket, ApiConstants.TopOffer));
            }

            Entities.Wallet? userWallet =  await GetUserWallet(request.UserId, cancellationToken);
            if (userWallet is null)
            {
                return Result<CreateResponse>.Failure(ErrorMessage.NotFound(ApiConstants.Wallet));
            }

            if (IsUserBalanceExceeded(userWallet.Balance, request.BetAmount))
            {
                return Result<CreateResponse>.Failure(
                    ErrorMessage.WalletBalanceExceeded());
            }
            
            RemoveRequiredBetFundsFromUsersWallet(userWallet, request.BetAmount);

            IEnumerable<decimal> selectedTipsQuotas = ExtractSelectedTipsQuotas(selectedTips);
            BetCalculationResultDto betCalculationResult = betCalculationService.CalculateBetInfo(request.BetAmount, selectedTipsQuotas);
            
            var ticket = new Entities.Ticket
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                BetAmount = betCalculationResult.FullBetPlaced,
                ManipulativeCost = betCalculationResult.ManipulativeCost,
                TotalQuota = betCalculationResult.TotalQuota,
                PossibleWinBeforeTax = betCalculationResult.PossibleWinBeforeTax,
                TaxAmount = betCalculationResult.TaxAmount,
                PossibleWinAfterTax = betCalculationResult.PossibleWinAfterTax,
                CreatedAt = DateTimeOffset.UtcNow,
                TicketSelections = selectedTips.Select(t => new TicketSelection
                {
                    Id = Guid.NewGuid(),
                    OfferId = t.OfferId,
                    ChosenTipCode = t.TipCode,
                    ChosenTipCodeQuota = (decimal)t.Quota!
                }).ToList()
            };
            
            await dbContext.Tickets.AddAsync(ticket, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            var response = new CreateResponse
            {
                TicketId = ticket.Id,
                BetAmount = ticket.BetAmount,
                ManipulativeCost = ticket.ManipulativeCost,
                TotalQuota = ticket.TotalQuota,
                PossibleWinBeforeTax = ticket.PossibleWinBeforeTax,
                TaxAmount = ticket.TaxAmount,
                PossibleWinAfterTax = ticket.PossibleWinAfterTax,
                TicketSelections = ticket.TicketSelections.Select(s => new TicketSelectionResponse
                {
                    OfferId = s.OfferId,
                    TipCode = s.ChosenTipCode,
                    Quota = s.ChosenTipCodeQuota
                }).ToList()
            };
      
            return Result<CreateResponse>.Success(response);
        }

        private static List<OfferTip> FilterTipsThatMatchRequestSelectionsOfferIdAndTipCode(
            IReadOnlyList<CreateTicketSelectionDto> requestSelections,
            List<OfferTip> selectedTips) =>
            selectedTips
                .Where(t => requestSelections.Any(s => s.OfferId == t.OfferId && s.TipCode == t.TipCode))
                .ToList();

        private static List<Guid> SelectOfferIdsFromRequestSelections(IReadOnlyList<CreateTicketSelectionDto> requestSelections) 
            => requestSelections.Select(s => s.OfferId).ToList();

        private async Task<List<OfferTip>> GetOfferTipsForSelectedOfferIds(IReadOnlyList<Guid> offerIds, CancellationToken cancellationToken) =>
            await dbContext.OfferTips
                .Where(t => offerIds.Contains(t.OfferId))
                .ToListAsync(cancellationToken);

        private async Task<int> GetTopOffersCount(IReadOnlyList<Guid> offerIds, CancellationToken cancellationToken) =>
            await dbContext.Offers
                .Where(o => offerIds.Contains(o.Id) && o.IsTopOffer)
                .CountAsync(cancellationToken);

        private static IEnumerable<decimal> ExtractSelectedTipsQuotas(IReadOnlyList<OfferTip> selectedTips)
            => selectedTips
                .Select(t => t.Quota!.Value)
                .AsEnumerable();

        private static void RemoveRequiredBetFundsFromUsersWallet(Entities.Wallet userWallet, decimal betAmount) 
            => userWallet.Balance -= betAmount;

        private async Task<Entities.Wallet?> GetUserWallet(Guid userId, CancellationToken cancellationToken) =>
            await dbContext.Wallets
                .Where(w => w.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken);
        
        private static bool IsUserBalanceExceeded(decimal userWalletBalance, decimal betAmount) =>
            userWalletBalance < betAmount;
    }
}
