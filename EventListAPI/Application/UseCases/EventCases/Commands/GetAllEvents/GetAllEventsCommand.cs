using MediatR;
using Application.DTOs.EventDTOs;
using System.Collections.Generic;

namespace Application.UseCases.EventCases.Commands.GetAllEvents
{
    public record GetAllEventsCommand() : IRequest<IEnumerable<GetEventDto>>;
}
