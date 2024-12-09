using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Application.DTOs.EventDTOs;

namespace Application.UseCases.EventCases.Commands.FilteredEvents
{
    public class GetFilteredEventsUseCase : IRequestHandler<GetFilteredEventsCommand, IEnumerable<GetEventDto>>
    {
        private readonly IEventRepository _eventRepository;

        public GetFilteredEventsUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<IEnumerable<GetEventDto>> Handle(GetFilteredEventsCommand request, CancellationToken cancellationToken)
        {
            var events = await _eventRepository.GetAllAsync();

            var filters = new List<Func<Event, bool>>();
            var eventFiltersDto = request.EventFiltersDto; 

            if (eventFiltersDto.EventDate.HasValue)
            {
                filters.Add(e => e.EventDate.Date == eventFiltersDto.EventDate.Value.Date);
            }

            if (!string.IsNullOrEmpty(eventFiltersDto.Location))
            {
                filters.Add(e => e.Location.Equals(eventFiltersDto.Location, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(eventFiltersDto.Category))
            {
                filters.Add(e => e.Category.Equals(eventFiltersDto.Category, StringComparison.OrdinalIgnoreCase));
            }

            foreach (var filter in filters)
            {
                events = events.Where(filter);
            }

            return events.Select(GetEventDto.MapIntoDto);
        }
    }
}
