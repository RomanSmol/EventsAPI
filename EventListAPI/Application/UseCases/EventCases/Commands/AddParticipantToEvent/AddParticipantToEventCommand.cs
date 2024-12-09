using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.EventCases.Commands.AddParticipantToEvent
{
    public class AddParticipantToEventCommand : IRequest<Unit>
    {
        public int EventId { get; set; }
        public int ParticipantId { get; set; }
    }
}
