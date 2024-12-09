using Application.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.EventCases.Commands.AddParticipantToEvent
{
    public class AddParticipantToEventUseCase : IRequestHandler<AddParticipantToEventCommand, Unit>
    {
        private readonly IEventParticipantRepository _eventParticipantRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IParticipantRepository _participantRepository;

        public AddParticipantToEventUseCase(
            IEventParticipantRepository eventParticipantRepository,
            IEventRepository eventRepository,
            IParticipantRepository participantRepository)
        {
            _eventParticipantRepository = eventParticipantRepository;
            _eventRepository = eventRepository;
            _participantRepository = participantRepository;
        }

        public async Task<Unit> Handle(AddParticipantToEventCommand request, CancellationToken cancellationToken)
        {
            if (!await _eventRepository.ExistsAsync(request.EventId, cancellationToken))
            {
                throw new NotFoundException("Event", request.EventId);
            }

            if (!await _participantRepository.ExistsAsync(request.ParticipantId, cancellationToken))
            {
                throw new NotFoundException("Participant", request.ParticipantId);
            }

            var eventParticipant = new EventParticipant
            {
                EventId = request.EventId,
                ParticipantId = request.ParticipantId,
                RegistrationDate = DateTime.UtcNow
            };

            await _eventParticipantRepository.AddAsync(eventParticipant);

            return Unit.Value;
        }
    }
}
