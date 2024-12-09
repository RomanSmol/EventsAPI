using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ParticipantDTOs
{
    public record GetParticipantDto(int Id, string FirstName, string LastName, DateTime DateOfBirth, DateTime RegistrationDate,
        string Email)
    {
        public static GetParticipantDto MapIntoDto(Participant participantEntity)
        {
            return new GetParticipantDto(participantEntity.Id, participantEntity.FirstName, participantEntity.LastName,
                participantEntity.DateOfBirth, participantEntity.RegistrationDate, participantEntity.Email);
        }
    }
}