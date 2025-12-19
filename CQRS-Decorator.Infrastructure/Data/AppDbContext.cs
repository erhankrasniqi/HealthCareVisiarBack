using CQRS_Decorator.Domain.Aggregates.AppointmentAggregate;
using CQRS_Decorator.Domain.Aggregates.DoctorAggregate;
using CQRS_Decorator.Domain.Aggregates.PatientAggregate;
using Microsoft.EntityFrameworkCore;

namespace CQRS_Decorator.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<AppointmentStatusEntity> AppointmentStatuses => Set<AppointmentStatusEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Doctor configuration
            modelBuilder.Entity<Doctor>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
                b.Property(x => x.LastName).IsRequired().HasMaxLength(100);
                b.Property(x => x.Specialization).IsRequired().HasMaxLength(100);
                b.Property(x => x.Email).IsRequired().HasMaxLength(200);
                b.Property(x => x.PhoneNumber).HasMaxLength(20);
                b.Property(x => x.LicenseNumber).IsRequired().HasMaxLength(50);
                
                b.HasIndex(x => x.Email).IsUnique();
                b.HasIndex(x => x.LicenseNumber).IsUnique();

                // Configure DateTime properties for UTC
                b.Property(x => x.CreatedOn).HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                
                b.Property(x => x.ModifiedOn).HasConversion(
                    v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);
            });

            // Patient configuration
            modelBuilder.Entity<Patient>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
                b.Property(x => x.LastName).IsRequired().HasMaxLength(100);
                b.Property(x => x.Email).IsRequired().HasMaxLength(200);
                b.Property(x => x.PhoneNumber).HasMaxLength(20);
                b.Property(x => x.Address).HasMaxLength(500);
                b.Property(x => x.PasswordHash).IsRequired().HasMaxLength(500);
                
                b.HasIndex(x => x.Email).IsUnique();

                // Configure DateTime properties for UTC
                b.Property(x => x.DateOfBirth).HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                
                b.Property(x => x.CreatedOn).HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                
                b.Property(x => x.ModifiedOn).HasConversion(
                    v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);
            });

            // Appointment configuration
            modelBuilder.Entity<Appointment>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.PatientId).IsRequired();
                b.Property(x => x.DoctorId).IsRequired();
                b.Property(x => x.AppointmentDate).IsRequired();
                b.Property(x => x.Reason).IsRequired().HasMaxLength(500);
                
                b.HasIndex(x => x.PatientId);
                b.HasIndex(x => x.DoctorId);
                b.HasIndex(x => x.AppointmentDate);
                b.HasIndex(x => x.AppointmentStatus);

                // Prevent duplicate appointments at the same time for the same doctor
                var scheduledStatusValue = (int)AppointmentStatus.Scheduled;
                b.HasIndex(x => new { x.DoctorId, x.AppointmentDate, x.StartTime, x.EndTime, x.AppointmentStatus })
                    .HasFilter($"\"AppointmentStatus\" = {scheduledStatusValue}") // Only for Scheduled appointments
                    .IsUnique();

                // Configure DateTime properties for UTC
                b.Property(x => x.AppointmentDate).HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                
                b.Property(x => x.CreatedOn).HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                
                b.Property(x => x.ModifiedOn).HasConversion(
                    v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);
            });

            // AppointmentStatus configuration
            modelBuilder.Entity<AppointmentStatusEntity>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(50);
                b.Property(x => x.Description).IsRequired().HasMaxLength(200);
            });
        }
    }
}