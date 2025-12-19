using CQRS_Decorator.Domain.Aggregates.PatientAggregate;
using CQRS_Decorator.Application.Common.Repositories;
using CQRS_Decorator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CQRS_Decorator.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Patient> GetByIdAsync(Guid id)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Patient> GetByEmailAsync(string email)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients.Where(p => p.Status == SharedKernel.EntityStatus.Active).ToListAsync();
        }

        public async Task AddAsync(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }
    }
}
