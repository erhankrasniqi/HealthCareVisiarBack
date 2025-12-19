using CQRS_Decorator.Domain.Aggregates.AppointmentAggregate;
using CQRS_Decorator.Application.Common.Repositories;
using CQRS_Decorator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CQRS_Decorator.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment> GetByIdAsync(Guid id)
        {
            return await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(Guid patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByDoctorIdAsync(Guid doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Appointments
                .Where(a => a.AppointmentDate >= startDate && a.AppointmentDate <= endDate)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<bool> IsDoctorAvailableAsync(Guid doctorId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            // Convert to UTC for comparison
            var dateUtc = date.ToUniversalTime().Date;

            var conflictingAppointments = await _context.Appointments
                .Where(a => a.DoctorId == doctorId
                    && a.AppointmentDate.Date == dateUtc
                    && a.AppointmentStatus == AppointmentStatus.Scheduled
                    && ((a.StartTime < endTime && a.EndTime > startTime)))
                .AnyAsync();

            return !conflictingAppointments;
        }

        public async Task AddAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
    }
}
