using HattrickApp.Api.Entities;
using Mapster;

namespace HattrickApp.Api.Features.Ticket.GetAllByUserId;

public class TicketMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Entities.Ticket, GetAllByUserIdResponse>()
            .Map(dest => dest.TicketSelections, src => src.TicketSelections);
        
        config.NewConfig<TicketSelection, TicketSelectionDto>()
            .Map(dest => dest.FirstCompetitor, src => src.Offer!.FirstCompetitor)
            .Map(dest => dest.SecondCompetitor, src => src.Offer!.SecondCompetitor);
    }
       
}
