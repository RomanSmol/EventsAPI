using Application.DTOs.EventDTOs;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.EventCases.Commands.GetEventById
{
    public class GetEventByIdUseCase : IRequestHandler<GetEventByIdCommand, GetEventDto>
    {
        private readonly IEventRepository _eventRepository;

        public GetEventByIdUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<GetEventDto> Handle(GetEventByIdCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(request.EventId);

            if (eventEntity == null)
                throw new Exception("Event not found");

            return GetEventDto.MapIntoDto(eventEntity);
        }
    }
}
