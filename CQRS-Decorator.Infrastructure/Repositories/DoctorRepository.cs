using CQRS_Decorator.Domain.Aggregates.DoctorAggregate;
using CQRS_Decorator.Application.Common.Repositories;
using CQRS_Decorator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CQRS_Decorator.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext _context;

        public DoctorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _context.Doctors.Where(d => d.Status == SharedKernel.EntityStatus.Active).ToListAsync();
        }

        public async Task<Doctor> GetByIdAsync(Guid id)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Doctor> GetByEmailAsync(string email)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.Email == email);
        }

        public async Task<Doctor> GetByLicenseNumberAsync(string licenseNumber)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.LicenseNumber == licenseNumber);
        }

        public async Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization)
        {
            return await _context.Doctors
                .Where(d => d.Specialization == specialization && d.Status == SharedKernel.EntityStatus.Active)
                .ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync()
        {
            return await _context.Doctors
                .Where(d => d.IsAvailable && d.Status == SharedKernel.EntityStatus.Active)
                .ToListAsync();
        }

        public async Task AddAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }
    }
}
