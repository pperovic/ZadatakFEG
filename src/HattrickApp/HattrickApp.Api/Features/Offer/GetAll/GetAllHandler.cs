using HattrickApp.Api.Common.Dtos;
using HattrickApp.Api.Common.ResultPattern;
using HattrickApp.Api.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HattrickApp.Api.Features.Offer.GetAll;

public static class GetAllHandler
{
    public record Query : GetAllRequest, IRequest<Result<PagedResultDto<GetAllResponse>>>;
    
    internal sealed class Handler(HattrickAppDbContext dbContext) : IRequestHandler<Query, Result<PagedResultDto<GetAllResponse>>>
    {
        public async Task<Result<PagedResultDto<GetAllResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            IQueryable<Entities.Offer> query = dbContext.Offers
                .AsNoTracking()
                .Include(o => o.Tips)
                .OrderBy(o => o.StartTime);
            
            int totalCount = await query.CountAsync(cancellationToken);

            List<Entities.Offer> offers = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            IEnumerable<GetAllResponse> allOffersDtoList = offers.Adapt<IEnumerable<GetAllResponse>>();

            var pagedResult = new PagedResultDto<GetAllResponse>
            {
                Items = allOffersDtoList,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
            
            return Result<PagedResultDto<GetAllResponse>>.Success(pagedResult);
        }
    }
}
