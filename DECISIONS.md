# Architectural Decisions & Reflection Log (DECISIONS.md)

## 1. Architecture & Design Patterns

- **Clean Architecture (4 Layers):**
  - `Domain`: Contains core entities (`Track`, `Artist`, `DSP`, `TrackDistribution`) and Domain Enums (`TrackStatus`). Zero dependencies.
  - `Application`: Contains DTOs, interfaces, business validation rules (using FluentValidation), and application logic.
  - `Infrastructure`: Handles DbContext (`AppDbContext`), EF Core migrations, Data Seeding, and SQL Server interactions.
  - `API`: ASP.NET Core Web API acting as the entry point, handling JWT Authentication, Routing, and Middleware.

## 2. Technical Decisions & Trade-offs

- **Database & ORM:** Entity Framework Core with SQL Server. Configured fluent mappings to enforce unique constraints (e.g., unique `ISRC` codes and unique `(TrackId, DspId)` distribution pairs).
- **Validation:** FluentValidation integrated into the Application layer to decouple request validation from business logic and keep Controllers lean.
- **Security:** JWT (JSON Web Token) authentication to protect sensitive endpoints such as track distribution actions.
- **DTO Mapping:** Handled explicit object mappings to eliminate circular JSON reference issues during serialization.

## 3. Data Seeding Strategy

- Seeded initial dummy data directly via EF Core `HasData` inside `OnModelCreating`. This ensures any evaluator running `dotnet ef database update` immediately gets a fully functional database populated with Artists, DSPs, and Tracks.

## 4. Known Trade-offs & Next Steps

- **Authentication Scope:** Implemented a simplified `AuthController` emitting signed JWT tokens for demonstration purposes. In production, this would connect to IdentityServer or Azure Entra ID.
