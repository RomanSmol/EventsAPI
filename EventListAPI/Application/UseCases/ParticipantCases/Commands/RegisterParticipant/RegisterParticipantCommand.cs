using Application.DTOs.ParticipantDTOs;
using MediatR;
using System;

namespace Application.UseCases.ParticipantCases.Commands.RegisterParticipant
{
    public class RegisterParticipantCommand : IRequest<ParticipantReadDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}