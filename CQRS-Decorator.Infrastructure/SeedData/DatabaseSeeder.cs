using CQRS_Decorator.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CQRS_Decorator.Infrastructure.SeedData
{
    public class DatabaseSeeder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(IServiceProvider serviceProvider, ILogger<DatabaseSeeder> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                _logger.LogInformation("Starting database seeding...");

                // Seed AppointmentStatuses first
                var appointmentStatusSeeder = new AppointmentStatusSeeder(context);
                await appointmentStatusSeeder.SeedAsync();
                _logger.LogInformation("AppointmentStatuses seeded successfully");

                // Seed Doctors
                var doctorSeeder = new DoctorSeeder(context);
                await doctorSeeder.SeedAsync();
                _logger.LogInformation("Doctors seeded successfully");

                _logger.LogInformation("Database seeding completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }
    }
}
