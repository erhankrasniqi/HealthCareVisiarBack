using Microsoft.Extensions.DependencyInjection;

namespace CQRS_Decorator.Infrastructure.SeedData
{
    public static class SeedDataExtensions
    {
        public static IServiceCollection AddDataSeeders(this IServiceCollection services)
        {
            services.AddScoped<IDataSeeder, DoctorSeeder>();
            services.AddScoped<IDataSeeder, AppointmentStatusSeeder>();
            services.AddScoped<DatabaseSeeder>();

            return services;
        }
    }
}
