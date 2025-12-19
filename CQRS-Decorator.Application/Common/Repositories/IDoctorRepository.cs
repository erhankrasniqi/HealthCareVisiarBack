using CQRS_Decorator.Domain.Aggregates.DoctorAggregate;

namespace CQRS_Decorator.Application.Common.Repositories
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor> GetByIdAsync(Guid id);
        Task<Doctor> GetByEmailAsync(string email);
        Task<Doctor> GetByLicenseNumberAsync(string licenseNumber);
        Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization);
        Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync();
        Task AddAsync(Doctor doctor);
        Task UpdateAsync(Doctor doctor);
    }
}
