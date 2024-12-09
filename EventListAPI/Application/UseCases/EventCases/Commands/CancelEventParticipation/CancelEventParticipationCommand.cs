using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.EventCases.Commands.CancelEventParticipation
{
    public record CancelEventParticipationCommand(int EventId, int ParticipantId) : IRequest<Unit>;
}
