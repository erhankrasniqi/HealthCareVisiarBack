# Healthcare Appointment System - CQRS & DDD

A modern healthcare appointment booking system built with .NET 9, implementing CQRS (Command Query Responsibility Segregation) and Domain-Driven Design patterns.

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-336791?style=flat&logo=postgresql)](https://www.postgresql.org/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## Features

- Patient registration and authentication with JWT
- Doctor management with specializations
- Appointment booking and management
- Automatic database creation and seeding
- CQRS pattern with Decorator pattern for cross-cutting concerns
- Domain-Driven Design with rich domain models
- Repository pattern for data access
- FluentValidation for request validation
- Swagger/OpenAPI documentation
- **Fixed ports (7036/5246)** for consistent API URL across environments

## Technologies

- **.NET 9** - Web API framework
- **PostgreSQL 16** - Database
- **Entity Framework Core 9** - ORM
- **JWT** - Authentication
- **FluentValidation** - Input validation
- **Swagger** - API documentation
- **Docker** - Containerization

## Architecture

```
API Layer ? Decorators ? Application Layer ? Domain Layer ? Infrastructure Layer
```

### CQRS Pattern

- **Commands** - Write operations (RegisterPatient, BookAppointment, CancelAppointment)
- **Queries** - Read operations (GetDoctors, GetAppointments, GetDoctorsLookup)

### Decorator Pattern

- **ValidationDecorator** - Validates commands before execution
- **LoggingDecorator** - Logs command execution

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL 16+](https://www.postgresql.org/download/)

### Installation

1. Clone the repository
```bash
git clone https://github.com/erhankrasniqi/HealthCareVisiarBack.git
cd HealthCareVisiarBack
```

2. Update database connection in `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DbConnection": "Host=localhost;Database=HealthCareDb;Username=postgres;Password=YOUR_PASSWORD;Port=5432"
  }
}
```

3. Run the application
```bash
cd CQRS-Decorator.API
dotnet run
```

The application will automatically:
- Create the database if it doesn't exist
- Apply all migrations
- Seed initial data (8 doctors, 4 appointment statuses)
- **Start on fixed ports: HTTPS (7036) and HTTP (5246)**

4. Access Swagger UI
```
https://localhost:7036/swagger
```

### API URLs

The API runs on **fixed ports** for consistency:

- **HTTPS (Production):** `https://localhost:7036`
- **HTTP (Development):** `http://localhost:5246`

**Frontend Configuration:**
```javascript
const API_BASE_URL = 'https://localhost:7036';
```

See [PORT-CONFIG.md](PORT-CONFIG.md) for detailed port configuration information.

## API Endpoints

### Authentication

```http
POST /api/auth/register - Register a new patient
POST /api/auth/login - Login and get JWT token
```

### Doctors

```http
GET /api/doctors - Get all doctors
GET /api/doctors/lookup - Get doctors for dropdown (ID, name, specialization)
```

### Appointments (Requires JWT)

```http
GET /api/appointments - Get my appointments
POST /api/appointments - Book an appointment
DELETE /api/appointments/{id} - Cancel an appointment
```

## Project Structure

```
??? CQRS-Decorator.API/              # Web API & Controllers
??? CQRS-Decorator.Application/      # Commands, Queries, Handlers
??? CQRS-Decorator.Domain/           # Domain Models & Business Logic
??? CQRS-Decorator.Infrastructure/   # Data Access & EF Core
??? CQRS-Decorator.Decorators/       # Validation & Logging Decorators
??? CQRS-Decorator.SharedKernel/     # Shared Components
```

## Docker Support

```bash
# Start with Docker Compose
docker-compose up -d

# Using Makefile
make up
make logs
make down
```

## Domain Model

### Aggregates

- **Patient** - Patient information and credentials
- **Doctor** - Doctor profiles with specializations
- **Appointment** - Appointment bookings with business rules

### Domain Events

- `PatientRegisteredEvent`
- `AppointmentBookedEvent`
- `AppointmentCancelledEvent`

## Security

- Password hashing with BCrypt
- JWT token-based authentication
- Patient ID extracted from JWT claims
- Protected endpoints with `[Authorize]` attribute

## Database

The application uses PostgreSQL with automatic migrations and seeding:

**Seeded Data:**
- 8 Doctors (Cardiology, Pediatrics, Dermatology, Orthopedics, Neurology, General Practice, Psychiatry, Ophthalmology)
- 4 Appointment Statuses (Scheduled, Completed, Cancelled, NoShow)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License.

## Author

**Erhan Krasniqi** - [GitHub](https://github.com/erhankrasniqi)
