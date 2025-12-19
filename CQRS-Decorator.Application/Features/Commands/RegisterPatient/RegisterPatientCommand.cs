using CQRS_Decorator.Application.Responses;
using CQRSDecorate.Net.Abstractions;

namespace CQRS_Decorator.Application.Features.Commands.RegisterPatient
{
    public record RegisterPatientCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string PhoneNumber,
        DateTime DateOfBirth,
        string Address
    ) : ICommand<GeneralResponse<Guid>>;
}
