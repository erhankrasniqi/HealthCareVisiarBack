using CQRS_Decorator.Application.Common.Abstractions;
using CQRS_Decorator.Domain.Aggregates.PatientAggregate;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CQRS_Decorator.Infrastructure.Auth
{
    public class PatientJwtTokenGenerator : IPatientJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public PatientJwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Patient patient)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiryInHours = int.Parse(jwtSettings["ExpiryInHours"] ?? "24");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, patient.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, patient.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, patient.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, patient.LastName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("patientId", patient.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expiryInHours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
