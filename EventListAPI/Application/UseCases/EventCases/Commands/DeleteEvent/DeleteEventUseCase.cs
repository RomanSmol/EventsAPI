using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.EventCases.Commands.DeleteEvent
{
    public class DeleteEventUseCase : IRequestHandler<DeleteEventCommand, Unit>
    {
        private readonly IEventRepository _eventRepository;

        public DeleteEventUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var eventToDelete = await _eventRepository.GetByIdAsync(request.Id);

            if (eventToDelete == null)
            {
                throw new KeyNotFoundException($"Event with ID {request.Id} not found.");
            }

            await _eventRepository.DeleteAsync(eventToDelete);
            return Unit.Value; 
        }
    }
}
