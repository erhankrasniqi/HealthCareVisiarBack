using CQRS_Decorator.Application.DTOs;
using CQRS_Decorator.Application.Responses;
using CQRSDecorate.Net.Abstractions;

namespace CQRS_Decorator.Application.Features.Commands.LoginPatient
{
    public record LoginPatientCommand(string Email, string Password)
        : ICommand<GeneralResponse<PatientAuthenticationResult>>;
}
