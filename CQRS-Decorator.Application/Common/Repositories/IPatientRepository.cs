using CQRS_Decorator.Domain.Aggregates.PatientAggregate;

namespace CQRS_Decorator.Application.Common.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient> GetByIdAsync(Guid id);
        Task<Patient> GetByEmailAsync(string email);
        Task<IEnumerable<Patient>> GetAllAsync();
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
    }
}
