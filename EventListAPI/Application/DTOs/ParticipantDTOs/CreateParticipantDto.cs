using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ParticipantDTOs
{
    public record CreateParticipantDto(string FirstName, string LastName, DateTime DateOfBirth, DateTime RegistrationDate,
        string Email, string Password)
    {
        public Participant MapIntoParticipantEntity()
        {
            return new Participant()
            {
                FirstName = FirstName,
                LastName = LastName,
                DateOfBirth = DateOfBirth,
                RegistrationDate = RegistrationDate,
                Email = Email,
                Password = Password
            };
        }
    }
}
