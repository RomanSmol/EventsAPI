using Application.DTOs.EventDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.EventCases.Commands.FilteredEvents
{
    public class GetFilteredEventsCommand : IRequest<IEnumerable<GetEventDto>>
    {
        public EventFiltersDto EventFiltersDto { get; }

        public GetFilteredEventsCommand(EventFiltersDto eventFiltersDto)
        {
            EventFiltersDto = eventFiltersDto;
        }
    }
}
