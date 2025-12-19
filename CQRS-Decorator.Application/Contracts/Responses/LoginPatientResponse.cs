namespace CQRS_Decorator.Application.Contracts.Responses
{
    public record LoginPatientResponse(
        Guid PatientId,
        string Email,
        string FullName,
        string Token,
        DateTime ExpiresAt);
}
