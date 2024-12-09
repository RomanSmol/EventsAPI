using System;
using Application.DTOs.ParticipantDTOs;
using MediatR;

namespace Application.UseCases.ParticipantCases.Commands.GetParticipantById
{
    public class GetParticipantByIdCommand : IRequest<GetParticipantDto>
    {
        public int ParticipantId { get; }

        public GetParticipantByIdCommand(int participantId)
        {
            ParticipantId = participantId;
        }
    }
}
