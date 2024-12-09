using Application.DTOs.EventDTOs;
using Application.UseCases.EventCases.Commands.GetEventById;
using Domain.Interfaces;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.UseCases.EventCases.Commands.GetEventByName
{
    public class GetEventByNameUseCase : IRequestHandler<GetEventByNameCommand, GetEventDto>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public GetEventByNameUseCase(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<GetEventDto> Handle(GetEventByNameCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.EventName))
            {
                throw new ArgumentNullException(nameof(request.EventName), "Event name cannot be null or empty.");
            }

            var eventEntity = await _eventRepository.GetByNameAsync(request.EventName);

            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event with name '{request.EventName}' not found.");
            }   

            return GetEventDto.MapIntoDto(eventEntity);
        }
    }
}
