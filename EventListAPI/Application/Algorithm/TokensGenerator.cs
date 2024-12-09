using Application.DTOs.TokensDTOs;
using Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Algorithm
{
    public class TokensGenerator(IConfiguration configuration) : ITokensGenerator
    {

        public Token GenerateAccessToken(Participant participant, IEnumerable<string> participantRoles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, participant.Id.ToString())
            };

            foreach (var role in participantRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenExpires = DateTime.UtcNow.AddMinutes(GetJwtSetting<double>("AccessTokenExpiresInMinutes"));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetJwtSetting<string>("Key")));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: tokenExpires,
                issuer: GetJwtSetting<string>("Issuer"),
                signingCredentials: signingCredentials);

            return new Token
            {
                Value = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expires = tokenExpires
            };
        }

        public Token GenerateRefreshToken()
        {
            return new Token
            {
                Value = Guid.NewGuid().ToString(),
                Expires = DateTime.UtcNow.AddMinutes(GetJwtSetting<double>("RefreshTokenExpiresInMinutes"))
            };
        }

        private T GetJwtSetting<T>(string key) => configuration.GetValue<T>($"JwtSettings:{key}");

    }
}
