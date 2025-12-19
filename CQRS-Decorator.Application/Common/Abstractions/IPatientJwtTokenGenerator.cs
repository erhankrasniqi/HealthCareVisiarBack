using CQRS_Decorator.Domain.Aggregates.PatientAggregate;

namespace CQRS_Decorator.Application.Common.Abstractions
{
    public interface IPatientJwtTokenGenerator
    {
        string GenerateToken(Patient patient);
    }
}
