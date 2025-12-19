using CQRS_Decorator.Application.Common.Abstractions;
using CQRS_Decorator.Application.DTOs;
using CQRS_Decorator.Application.Responses;
using CQRS_Decorator.Application.Common.Repositories;
using CQRSDecorate.Net.Abstractions;

namespace CQRS_Decorator.Application.Features.Commands.LoginPatient
{
    public class LoginPatientCommandHandler : ICommandHandler<LoginPatientCommand, GeneralResponse<PatientAuthenticationResult>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPatientJwtTokenGenerator _jwtTokenGenerator;

        public LoginPatientCommandHandler(
            IPatientRepository patientRepository,
            IPasswordHasher passwordHasher,
            IPatientJwtTokenGenerator jwtTokenGenerator)
        {
            _patientRepository = patientRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<GeneralResponse<PatientAuthenticationResult>> HandleAsync(LoginPatientCommand command)
        {
            var patient = await _patientRepository.GetByEmailAsync(command.Email);

            if (patient == null)
            {
                return new GeneralResponse<PatientAuthenticationResult>
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            if (!_passwordHasher.VerifyPassword(command.Password, patient.PasswordHash))
            {
                return new GeneralResponse<PatientAuthenticationResult>
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            var token = _jwtTokenGenerator.GenerateToken(patient);

            var result = new PatientAuthenticationResult
            {
                PatientId = patient.Id,
                Email = patient.Email,
                FullName = patient.GetFullName(),
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            return new GeneralResponse<PatientAuthenticationResult>
            {
                Success = true,
                Message = "Login successful",
                Result = result
            };
        }
    }
}
