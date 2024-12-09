using MediatR;
using Application.DTOs.EventDTOs;

namespace Application.UseCases.EventCases.Commands.UpdateEvent
{
    public class UpdateEventCommand : IRequest<bool> 
    {
        public UpdateEventDto UpdateEventDto { get; }

        public UpdateEventCommand(UpdateEventDto updateEventDto)
        {
            UpdateEventDto = updateEventDto;
        }
    }
}
