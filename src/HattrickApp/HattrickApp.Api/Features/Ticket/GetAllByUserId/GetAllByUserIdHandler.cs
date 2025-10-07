using HattrickApp.Api.Common.Dtos;
using HattrickApp.Api.Common.ResultPattern;
using HattrickApp.Api.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HattrickApp.Api.Features.Ticket.GetAllByUserId;

public static class GetAllByUserIdHandler
{
    public record Query : GetAllByUserIdRequest, IRequest<Result<PagedResultDto<GetAllByUserIdResponse>>>;
    
    internal sealed class Handler(HattrickAppDbContext dbContext) : IRequestHandler<Query, Result<PagedResultDto<GetAllByUserIdResponse>>>
    {
        public async Task<Result<PagedResultDto<GetAllByUserIdResponse>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            IOrderedQueryable<Entities.Ticket> query = dbContext.Tickets
                .AsNoTracking()
                .Include(x => x.TicketSelections)
                .ThenInclude(ts => ts.Offer)
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.CreatedAt);
            
            int totalCount = await query.CountAsync(cancellationToken);
            
            IEnumerable<Entities.Ticket> tickets = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            
            IEnumerable<GetAllByUserIdResponse> allUserTicketsDtosList = tickets.Adapt<IEnumerable<GetAllByUserIdResponse>>();
            
            var pagedResult = new PagedResultDto<GetAllByUserIdResponse>
            {
                Items = allUserTicketsDtosList,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
            
            return Result<PagedResultDto<GetAllByUserIdResponse>>.Success(pagedResult);
        }
    }
}
