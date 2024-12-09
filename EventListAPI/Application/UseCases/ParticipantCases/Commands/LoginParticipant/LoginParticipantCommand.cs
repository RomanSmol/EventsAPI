using Application.DTOs.TokensDTOs;
using MediatR;

namespace Application.UseCases.ParticipantCases.Commands.LoginParticipant
{
    public class LoginParticipantCommand : IRequest<TokensReadDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
