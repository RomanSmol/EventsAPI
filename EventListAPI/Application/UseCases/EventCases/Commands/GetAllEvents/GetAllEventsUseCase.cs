using Application.DTOs.EventDTOs;
using Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.EventCases.Commands.GetAllEvents
{
    public class GetAllEventsUseCase : IRequestHandler<GetAllEventsCommand, IEnumerable<GetEventDto>>
    {
        private readonly IEventRepository _eventRepository;

        public GetAllEventsUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<IEnumerable<GetEventDto>> Handle(GetAllEventsCommand request, CancellationToken cancellationToken)
        {
            var events = await _eventRepository.GetAllAsync();

            return events.Select(GetEventDto.MapIntoDto).ToList();
        }
    }
}
