using Application.DTOs.EventDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.EventCases.Commands.GetEventByName
{
    public class GetEventByNameCommand : IRequest<GetEventDto>
    {
        public string EventName { get; }

        public GetEventByNameCommand(string eventName)
        {
            EventName = eventName;
        }
    }
}
