using CQRS_Decorator.Application.Common.Abstractions;
using CQRS_Decorator.Application.Responses;
using CQRS_Decorator.Domain.Aggregates.PatientAggregate;
using CQRS_Decorator.Application.Common.Repositories;
using CQRSDecorate.Net.Abstractions;

namespace CQRS_Decorator.Application.Features.Commands.RegisterPatient
{
    public class RegisterPatientCommandHandler : ICommandHandler<RegisterPatientCommand, GeneralResponse<Guid>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterPatientCommandHandler(
            IPatientRepository patientRepository,
            IPasswordHasher passwordHasher)
        {
            _patientRepository = patientRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<GeneralResponse<Guid>> HandleAsync(RegisterPatientCommand command)
        {
            var existingPatient = await _patientRepository.GetByEmailAsync(command.Email);
            if (existingPatient != null)
            {
                return new GeneralResponse<Guid>
                {
                    Success = false,
                    Message = "A patient with this email already exists"
                };
            }

            var passwordHash = _passwordHasher.HashPassword(command.Password);

            var patient = Patient.Create(
                command.FirstName,
                command.LastName,
                command.Email,
                command.PhoneNumber,
                command.DateOfBirth,
                command.Address,
                passwordHash);

            await _patientRepository.AddAsync(patient);

            return new GeneralResponse<Guid>
            {
                Success = true,
                Message = "Patient registered successfully",
                Result = patient.Id
            };
        }
    }
}
