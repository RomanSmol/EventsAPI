using Application.DTOs.EventDTOs;
using Domain.Models;
using MediatR;
using System;

namespace Application.UseCases.EventCases.Commands.CreateEvent
{
    public record CreateEventCommand(CreateEventDto EventDto) : IRequest<int> 
    {
            public Event MapIntoEventEntity() => EventDto.MapIntoEventEntity();
    }
}
