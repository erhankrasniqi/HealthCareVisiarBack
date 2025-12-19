using System.ComponentModel.DataAnnotations;

namespace CQRS_Decorator.Application.Contracts.Requests
{
    public record RegisterPatientRequest(
        [Required] string FirstName,
        [Required] string LastName,
        [Required][EmailAddress] string Email,
        [Required] string Password,
        [Required] string PhoneNumber,
        [Required] DateTime DateOfBirth,
        [Required] string Address);
}
