using Application.DTOs.TokensDTOs;
using Application.Interfaces;
using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Application.Validators;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.ParticipantCases.Commands.LoginParticipant
{
    public class LoginParticipantUseCase : IRequestHandler<LoginParticipantCommand, TokensReadDto>
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokensGenerator _tokensGenerator;
        private readonly IMapper _mapper;
        private readonly ParticipantLoginValidator _participantLoginValidator;

        public LoginParticipantUseCase(
            IParticipantRepository participantRepository,
            IPasswordHasher passwordHasher,
            ITokensGenerator tokensGenerator,
            IMapper mapper,
            ParticipantLoginValidator participantValidator)
        {
            _participantRepository = participantRepository;
            _passwordHasher = passwordHasher;
            _tokensGenerator = tokensGenerator;
            _mapper = mapper;
            _participantLoginValidator = participantValidator;
        }

        public async Task<TokensReadDto> Handle(LoginParticipantCommand request, CancellationToken cancellationToken)
        {
            var participantForValidation = new Participant
            {
                Email = request.Email,
                Password = request.Password
            };

            var validationResult = await _participantLoginValidator.ValidateAsync(participantForValidation, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var participant = (await _participantRepository.GetByPredicateAsync(
                participant => participant.Email == request.Email, cancellationToken)).FirstOrDefault();

            if (participant is null)
            {
                throw new NotFoundException(typeof(Participant).ToString(), request.Email);
            }

            if (!_passwordHasher.VerifyHashedPassword(participant.Password, request.Password))
            {
                throw new UnauthorizedException("Wrong password");
            }

            var accessToken = _tokensGenerator.GenerateAccessToken(participant, new List<string> { participant.Role.Name });
            var refreshToken = _tokensGenerator.GenerateRefreshToken();

            return new TokensReadDto
            {
                AccessToken = accessToken.Value,
                RefreshToken = refreshToken.Value,
                ExpiresIn = accessToken.Expires
            };
        }
    }
}
