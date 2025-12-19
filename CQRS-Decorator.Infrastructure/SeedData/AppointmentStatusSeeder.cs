using CQRS_Decorator.Infrastructure.Data;

namespace CQRS_Decorator.Infrastructure.SeedData
{
    public class AppointmentStatusSeeder : DataSeeder
    {
        public AppointmentStatusSeeder(AppDbContext context) : base(context)
        {
        }

        public override async Task SeedAsync()
        {
            if (await Task.Run(() => _context.AppointmentStatuses.Any()))
            {
                return; // Data already seeded
            }

            var statuses = new List<AppointmentStatusEntity>
            {
                new AppointmentStatusEntity
                {
                    Id = 1,
                    Name = "Scheduled",
                    Description = "Appointment is scheduled and confirmed"
                },
                new AppointmentStatusEntity
                {
                    Id = 2,
                    Name = "Completed",
                    Description = "Appointment was completed successfully"
                },
                new AppointmentStatusEntity
                {
                    Id = 3,
                    Name = "Cancelled",
                    Description = "Appointment was cancelled by patient or doctor"
                },
                new AppointmentStatusEntity
                {
                    Id = 4,
                    Name = "NoShow",
                    Description = "Patient did not show up for the appointment"
                }
            };

            await _context.AppointmentStatuses.AddRangeAsync(statuses);
            await _context.SaveChangesAsync();
        }
    }
}
