using MediatR;
using Application.DTOs.EventDTOs;

namespace Application.UseCases.EventCases.Commands.GetEventById
{
    public class GetEventByIdCommand : IRequest<GetEventDto>
    {
        public int EventId { get; }

        public GetEventByIdCommand(int eventId)
        {
            EventId = eventId;
        }
    }
}
