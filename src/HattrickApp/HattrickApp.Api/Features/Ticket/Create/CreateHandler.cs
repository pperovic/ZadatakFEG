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
            
            var offerIds = request.Selections.Select(s => s.OfferId).ToList();

            List<OfferTip> selectedTips = await dbContext.OfferTips
                .Where(t => offerIds.Contains(t.OfferId))
                .ToListAsync(cancellationToken);

            // validate in memory: only keep tips that match both OfferId + TipCode
            selectedTips = selectedTips
                .Where(t => request.Selections.Any(s => s.OfferId == t.OfferId && s.TipCode == t.TipCode))
                .ToList();

            if (selectedTips.Count != request.Selections.Count)
            {
                return Result<CreateResponse>.Failure(ErrorMessage.NotFound(ApiConstants.SelectedTips));
            }

            if (selectedTips.Any(t => t.Quota == null))
            {
                return Result<CreateResponse>.Failure(ErrorMessage.InvalidValues(ApiConstants.SelectedTips,
                    ApiConstants.Quota));
            }

            int topOffersCount = await dbContext.Offers
                .Where(o => offerIds.Contains(o.Id) && o.IsTopOffer)
                .CountAsync(cancellationToken);

            if (topOffersCount > 1)
            {
                return Result<CreateResponse>.Failure(
                    ErrorMessage.MoreThanOneNotAllowed(ApiConstants.Ticket, ApiConstants.TopOffer));
            }

            var quotas = selectedTips.Select(t => t.Quota!.Value).ToList();
            BetCalculationResultDto betResult = betCalculationService.Calculate(request.BetAmount, quotas);
            
            var ticket = new Entities.Ticket
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                BetAmount = betResult.FullBetPlaced,
                ManipulativeCost = betResult.ManipulativeCost,
                TotalQuota = betResult.TotalQuota,
                PossibleWinBeforeTax = betResult.PossibleWinBeforeTax,
                TaxAmount = betResult.TaxAmount,
                PossibleWinAfterTax = betResult.PossibleWinAfterTax,
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
    }
}
