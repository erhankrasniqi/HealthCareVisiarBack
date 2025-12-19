namespace CQRS_Decorator.Application.Contracts.Responses
{
    public record RegisterPatientResponse(
        Guid PatientId,
        string FirstName,
        string LastName,
        string Email,
        DateTime RegisteredAt);
}
