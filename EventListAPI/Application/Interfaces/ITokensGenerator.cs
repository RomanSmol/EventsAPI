using Application.DTOs.TokensDTOs;
using Domain.Models;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ITokensGenerator
    {
        Token GenerateAccessToken(Participant participant, IEnumerable<string> participantRoles);
        Token GenerateRefreshToken();
    }
}