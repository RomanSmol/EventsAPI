using Application.DTOs.ParticipantDTOs;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.EventParticipantCases.Commands.GetEventParticipants
{
    public class GetEventParticipantsUseCase : IRequestHandler<GetEventParticipantsCommand, IEnumerable<GetParticipantDto>>
    {
        private readonly IEventRepository _eventRepository;

        public GetEventParticipantsUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<IEnumerable<GetParticipantDto>> Handle(GetEventParticipantsCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(request.EventId);

            if (eventEntity == null)
                throw new KeyNotFoundException($"Event with ID {request.EventId} not found.");

            var participants = eventEntity.EventParticipants
                .Select(ep => GetParticipantDto.MapIntoDto(ep.Participant))
                .ToList();

            return participants;
        }
    }
}
