using Application.Validators;
using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.EventCases.Commands.UpdateEvent
{
    public class UpdateEventUseCase : IRequestHandler<UpdateEventCommand, bool>
    {
        private readonly IEventRepository _eventRepository;
        private readonly EventValidator _eventValidator;

        public UpdateEventUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
            _eventValidator = new EventValidator(); 
        }

        public async Task<bool> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var existingEvent = await _eventRepository.GetByIdAsync(request.UpdateEventDto.Id);

            if (existingEvent == null)
            {
                throw new KeyNotFoundException($"Event with ID {request.UpdateEventDto.Id} not found.");
            }

            request.UpdateEventDto.MapToEntity(existingEvent);

            var validationResult = await _eventValidator.ValidateAsync(existingEvent, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            await _eventRepository.UpdateAsync(existingEvent);
            return true;
        }
    }
}
