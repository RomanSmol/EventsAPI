using Application.Validators;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.EventCases.Commands.CreateEvent
{
    public class CreateEventUseCase : IRequestHandler<CreateEventCommand, int>
    {
        private readonly IEventRepository _eventRepository;
        private readonly EventValidator _eventValidator;

        public CreateEventUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
            _eventValidator = new EventValidator();
        }

        public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var newEvent = request.MapIntoEventEntity();

            var validationResult = await _eventValidator.ValidateAsync(newEvent, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            await _eventRepository.AddAsync(newEvent);
            return newEvent.Id;
        }
    }
}
