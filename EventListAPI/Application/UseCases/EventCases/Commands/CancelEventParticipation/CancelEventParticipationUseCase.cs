using Domain.Interfaces;
using Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.EventCases.Commands.CancelEventParticipation
{
    public class CancelEventParticipationUseCase : IRequestHandler<CancelEventParticipationCommand, Unit>
    {
        private readonly IEventParticipantRepository _eventParticipantRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IParticipantRepository _participantRepository;

        public CancelEventParticipationUseCase(IEventParticipantRepository eventParticipantRepository,IEventRepository eventRepository,IParticipantRepository participantRepository)
        {
            _eventParticipantRepository = eventParticipantRepository;
            _eventRepository = eventRepository;
            _participantRepository = participantRepository;
        }

        public async Task<Unit> Handle(CancelEventParticipationCommand request, CancellationToken cancellationToken)
        {
            if (!await _eventRepository.ExistsAsync(request.EventId, cancellationToken))
            {
                throw new NotFoundException("Event", request.EventId);
            }

            if (!await _participantRepository.ExistsAsync(request.ParticipantId, cancellationToken))
            {
                throw new NotFoundException("Participant", request.ParticipantId);
            }

            var eventParticipant = await _eventParticipantRepository.GetByEventAndParticipantIdAsync(request.EventId, request.ParticipantId, cancellationToken);
            if (eventParticipant == null)
            {
                throw new NotFoundException("EventParticipation", $"EventId: {request.EventId}, ParticipantId: {request.ParticipantId}");
            }

            await _eventParticipantRepository.DeleteAsync(eventParticipant, cancellationToken);

            return Unit.Value;
        }
    }
}
