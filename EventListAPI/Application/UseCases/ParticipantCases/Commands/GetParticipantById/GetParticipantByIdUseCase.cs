using Application.DTOs.EventDTOs;
using Application.DTOs.ParticipantDTOs;
using Application.UseCases.EventCases.Commands.GetEventById;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.ParticipantCases.Commands.GetParticipantById
{
    public class GetParticipantByIdUseCase : IRequestHandler<GetParticipantByIdCommand, GetParticipantDto>
    {
        private readonly IParticipantRepository _participantRepository;

        public GetParticipantByIdUseCase(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public async Task<GetParticipantDto> Handle(GetParticipantByIdCommand request, CancellationToken cancellationToken)
        {
            var participantEntity = await _participantRepository.GetByIdAsync(request.ParticipantId);

            if (participantEntity == null)
                throw new Exception("Participant not found");

            return GetParticipantDto.MapIntoDto(participantEntity);
        }
    }
}
