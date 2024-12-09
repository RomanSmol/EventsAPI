using MediatR;
using Application.DTOs.ParticipantDTOs;

namespace Application.UseCases.EventParticipantCases.Commands.GetEventParticipants
{
    public record GetEventParticipantsCommand(int EventId) : IRequest<IEnumerable<GetParticipantDto>>;
}
