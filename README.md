# ClinicFlow

A full-stack clinic management system for managing patients, appointments, providers, clinics, and allergies.

## Repository Structure

```
ClinicFlow/
├── Backend/            # ASP.NET Core 10 REST API
├── Frontend/           # Next.js web application
└── ClinicFlow.Tests/   # xUnit test project
```

---

## How to Run

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- Oracle Database instance

### Backend

```bash
cd Backend

# Restore dependencies
dotnet restore

# Set your Oracle connection string in appsettings.Development.json
# "OracleConnection": "your-connection-string"

# Run the API
dotnet run
```

API runs at `http://localhost:5064`.

### Frontend

```bash
cd Frontend

# Install dependencies
npm install

# Create .env.local and set the API URL
echo "NEXT_PUBLIC_API_URL=http://localhost:5064" > .env.local

# Start the dev server
npm run dev
```

Frontend runs at `http://localhost:3000`.

### Tests

```bash
cd ClinicFlow.Tests
dotnet test
```

---

## Design Choices

- **Layered architecture**: Controllers → Services → Repositories, keeping each layer focused on a single responsibility and making unit testing straightforward.
- **Repository pattern**: Abstracts database access behind interfaces, allowing services to be tested with mocked repositories without hitting the database.
- **Oracle + EF Core**: Oracle was chosen as the database with EF Core as the ORM, mapping models to existing Oracle table conventions (uppercase column names).
- **DTOs**: Separate request/response DTOs decouple the API contract from internal models, preventing over-posting and unintended data exposure.
- **Global exception middleware**: Centralised error handling via `ExceptionMiddleware` returns consistent JSON error responses for all custom exceptions.
- **Next.js (App with Pages Router)**: Chosen for its file-based routing and simple deployment story alongside a service layer that wraps all API calls.

---

## Assumptions

- This is an **administrator-only portal**. There is a single admin user who manages all records — patients, clinics, providers, allergies, and appointments. There is no patient-facing or provider-facing interface.
- Clinics and providers are predefined, but the admin has full CRUD capability over them — they can be added, updated, or removed as needed.
- Each provider belongs to exactly **one clinic** (many providers → one clinic). A provider cannot span multiple clinics.
- Each appointment is tied to exactly one patient, one provider, and one clinic.
- A patient can have multiple allergies, managed via the `PatientAllergy` join entity.
- Authentication is handled via a simple login endpoint; the session is stored in `localStorage`.
- The Oracle schema already exists — EF Core is used in a database-first style (no migrations are applied on startup).
- CORS is permissive only for `http://localhost:3000` (the local frontend); production deployment would require updating this.

---

## Tradeoffs

| Decision | Tradeoff |
|---|---|
| Oracle over a lighter DB (e.g. PostgreSQL) | Matches enterprise requirement but adds licensing/setup complexity for local dev |
| No JWT authentication | Simpler to implement but less secure; `localStorage` sessions are vulnerable to XSS |
| Repository pattern with manual DI registration | Explicit and clear but verbose; a generic repository could reduce boilerplate |
| No pagination on list endpoints | Simpler API surface but will degrade at scale with large datasets |
| CSS Modules over a component library | Full style control but more CSS to maintain compared to Tailwind or MUI |

---

## API Documentation

Full API reference with request/response examples:

[ClinicFlow API](https://documenter.getpostman.com/view/56006366/2sBY4Jx3H5)
