using System.ComponentModel.DataAnnotations;

namespace CQRS_Decorator.Application.Contracts.Requests
{
    public record LoginPatientRequest(
        [Required][EmailAddress] string Email,
        [Required] string Password);
}
