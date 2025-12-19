# Database Seeding

This folder contains all database seeding logic for the CQRS-Decorator application.

## Structure

```
SeedData/
??? IDataSeeder.cs              # Interface for data seeders
??? DataSeeder.cs               # Base class for all seeders
??? DoctorSeeder.cs             # Seeds doctor data
??? AppointmentStatusSeeder.cs  # Seeds appointment status data
??? DatabaseSeeder.cs           # Main seeder orchestrator
??? SeedDataExtensions.cs       # DI registration extensions
```

## How It Works

1. **IDataSeeder**: Defines the contract for all seeder classes
2. **DataSeeder**: Abstract base class providing common functionality
3. **Specific Seeders**: Each entity has its own seeder (DoctorSeeder, AppointmentStatusSeeder)
4. **DatabaseSeeder**: Orchestrates all seeders in the correct order
5. **SeedDataExtensions**: Registers all seeders in DI container

## Seeded Data

### Doctors (8 records)
- Dr. Sarah Johnson - Cardiology (15 years experience)
- Dr. Michael Chen - Pediatrics (10 years experience)
- Dr. Emily Williams - Dermatology (8 years experience)
- Dr. Robert Brown - Orthopedics (20 years experience)
- Dr. Lisa Martinez - Neurology (12 years experience)
- Dr. David Anderson - General Practice (18 years experience)
- Dr. Jennifer Taylor - Psychiatry (14 years experience)
- Dr. James Wilson - Ophthalmology (16 years experience)

### AppointmentStatuses (4 records)
- 1 = Scheduled - Appointment is scheduled and confirmed
- 2 = Completed - Appointment was completed successfully
- 3 = Cancelled - Appointment was cancelled by patient or doctor
- 4 = NoShow - Patient did not show up for the appointment

## Usage

The seeding is automatically executed on application startup in `Program.cs`:

```csharp
// Register seeders
builder.Services.AddDataSeeders();

// Execute seeding
var seeder = services.GetRequiredService<DatabaseSeeder>();
await seeder.SeedAsync();
```

## Adding New Seeders

1. Create a new seeder class inheriting from `DataSeeder`
2. Implement the `SeedAsync()` method
3. Register it in `SeedDataExtensions.cs`
4. Add it to `DatabaseSeeder.cs` in the correct order

Example:

```csharp
public class PatientSeeder : DataSeeder
{
    public PatientSeeder(AppDbContext context) : base(context)
    {
    }

    public override async Task SeedAsync()
    {
        if (await _context.Patients.AnyAsync())
        {
            return; // Data already seeded
        }

        var patients = new List<Patient>
        {
            // Add patient data
        };

        await _context.Patients.AddRangeAsync(patients);
        await _context.SaveChangesAsync();
    }
}
```

## Notes

- Seeders check if data already exists before inserting
- Seeding runs after migrations are applied
- In development mode, the database is recreated on each startup
- All seeding operations are logged for debugging
