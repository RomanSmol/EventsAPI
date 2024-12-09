using Application.DTOs.ParticipantDTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.UseCases.ParticipantCases.Commands.RegisterParticipant
{
    public class RegisterParticipantUseCase(IParticipantRepository participantRepository, IPasswordHasher passwordHasher, IMapper mapper, ParticipantValidator participantValidator) : IRequestHandler<RegisterParticipantCommand, ParticipantReadDto>
    {
        private readonly ParticipantValidator _participantValidator = participantValidator;

        public async Task<ParticipantReadDto> Handle(RegisterParticipantCommand request, CancellationToken cancellationToken)
        {
            var newParticipant = mapper.Map<Participant>(request);

            var validationResult = await _participantValidator.ValidateAsync(newParticipant, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            newParticipant.Password = passwordHasher.HashPassword(request.Password);
            await participantRepository.AddAsync(newParticipant, cancellationToken);
            await participantRepository.SaveChangesAsync(cancellationToken);

            return mapper.Map<ParticipantReadDto>(newParticipant);
        }
    }
}
