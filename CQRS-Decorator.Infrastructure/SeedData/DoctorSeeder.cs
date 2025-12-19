using CQRS_Decorator.Domain.Aggregates.DoctorAggregate;
using CQRS_Decorator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CQRS_Decorator.Infrastructure.SeedData
{
    public class DoctorSeeder : DataSeeder
    {
        public DoctorSeeder(AppDbContext context) : base(context)
        {
        }

        public override async Task SeedAsync()
        {
            if (await _context.Doctors.AnyAsync())
            {
                return; // Data already seeded
            }

            var doctors = new List<Doctor>
            {
                Doctor.Create(
                    "Sarah",
                    "Johnson",
                    "Cardiology",
                    "sarah.johnson@hospital.com",
                    "+1-555-0101",
                    "LIC-CARD-001",
                    15
                ),
                Doctor.Create(
                    "Michael",
                    "Chen",
                    "Pediatrics",
                    "michael.chen@hospital.com",
                    "+1-555-0102",
                    "LIC-PEDI-002",
                    10
                ),
                Doctor.Create(
                    "Emily",
                    "Williams",
                    "Dermatology",
                    "emily.williams@hospital.com",
                    "+1-555-0103",
                    "LIC-DERM-003",
                    8
                ),
                Doctor.Create(
                    "Robert",
                    "Brown",
                    "Orthopedics",
                    "robert.brown@hospital.com",
                    "+1-555-0104",
                    "LIC-ORTH-004",
                    20
                ),
                Doctor.Create(
                    "Lisa",
                    "Martinez",
                    "Neurology",
                    "lisa.martinez@hospital.com",
                    "+1-555-0105",
                    "LIC-NEUR-005",
                    12
                ),
                Doctor.Create(
                    "David",
                    "Anderson",
                    "General Practice",
                    "david.anderson@hospital.com",
                    "+1-555-0106",
                    "LIC-GENP-006",
                    18
                ),
                Doctor.Create(
                    "Jennifer",
                    "Taylor",
                    "Psychiatry",
                    "jennifer.taylor@hospital.com",
                    "+1-555-0107",
                    "LIC-PSYC-007",
                    14
                ),
                Doctor.Create(
                    "James",
                    "Wilson",
                    "Ophthalmology",
                    "james.wilson@hospital.com",
                    "+1-555-0108",
                    "LIC-OPHT-008",
                    16
                )
            };

            await _context.Doctors.AddRangeAsync(doctors);
            await _context.SaveChangesAsync();
        }
    }
}
