# ClinicFlow - Backend

A RESTful API for clinic management built with ASP.NET Core 10, using Oracle Database via Entity Framework Core.

## Tech Stack

- **Framework**: ASP.NET Core 10
- **ORM**: Entity Framework Core 10
- **Database**: Oracle
- **Password Hashing**: BCrypt.Net-Next
- **API Docs**: Postmans

## Getting Started

### Prerequisites

- .NET 10 SDK
- Oracle Database instance

### Installation

```bash
# Restore dependencies
dotnet restore

# Run the API
dotnet run
```

The API runs on `http://localhost:5064` by default.

### Configuration

Set the Oracle connection string in `appsettings.json` or `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "your-oracle-connection-string"
  }
}
```

## Project Structure

```
Backend/
├── Controllers/        # API endpoints
├── Services/           # Business logic layer
├── Repositories/       # Data access layer
├── Models/             # Entity models
├── Dto/                # Request/Response DTOs
├── Data/               # EF Core DbContext
├── Exceptions/         # Custom exceptions and middleware
└── Properties/         # Launch settings
```

## API Endpoints

| Resource         | Base Route              |
|------------------|-------------------------|
| Users            | `/api/user`             |
| Patients         | `/api/patient`          |
| Providers        | `/api/provider`         |
| Clinics          | `/api/clinic`           |
| Appointments     | `/api/appointment`      |
| Allergies        | `/api/allergy`          |
| Patient Allergies| `/api/patientallergy`   |

## Architecture

Follows a layered architecture:
- **Controllers** handle HTTP requests and delegate to Services
- **Services** contain business logic and delegate to Repositories
- **Repositories** handle all database operations via EF Core

## CORS

Configured to allow requests from `http://localhost:3000` (the Next.js frontend).

## License

This project is private and proprietary.
