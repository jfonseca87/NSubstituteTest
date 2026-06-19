# NSubstituteTest

A .NET 9 Web API that demonstrates the **Repository + Service** pattern with **Dapper** for data access and **memory caching** for performance. The project includes a comprehensive xUnit test suite using **NSubstitute** to mock dependencies, showcasing how to write isolated unit tests for service and repository layers.

The API exposes a single endpoint (`GET /api/tasks`) that returns tasks from a SQL Server database. The `TaskService` uses a cache-aside pattern: it first checks an `ICacheService` (backed by `IMemoryCache`), and only falls back to the repository if the data is not cached or has expired (5-minute TTL).

## Technologies / Libraries

- **.NET 9** (ASP.NET Core Minimal API)
- **Dapper** 2.1.66 — Micro-ORM for SQL Server
- **Microsoft.Data.SqlClient** 6.1.2 — SQL Server driver
- **Microsoft.AspNetCore.OpenApi** 9.0.9 — OpenAPI support
- **NSubstitute** 5.3.0 — Mocking framework (tests)
- **xUnit** 2.9.3 — Test framework

## Key Features

- **Repository layer** — `ITaskRepository` / `TaskRepository` with Dapper for raw SQL queries (SELECT TOP 1000).
- **Service layer** — `ITaskService` / `TaskService` with cache-aside logic.
- **Caching** — `ICacheService` / `MemoryCacheService` wraps `IMemoryCache` with a generic `GetOrSetAsync<T>` pattern.
- **Connection factory** — `IConnectionFactory` / `SqlServerConnectionFactory` abstracts `SqlConnection` creation.
- **Unit tests** (`NSubstitute.UnitTest`) — 7 test cases covering:
  - `TaskServiceTest` — cache hit, cache miss, null cache returns empty, cancellation token propagation, repository exception propagation.
  - `CalculatorServicesTest` — basic arithmetic mocking and exception simulation.
  - `TaskRepositoryTest` — verifies repository returns expected data.

## How to Run

1. Update the connection string in `appsettings.json` to point to your SQL Server instance.
2. Create the `Tasks` table and seed data as expected by the repository query.
3. Run the API:
   ```bash
   dotnet run --project NSubstituteTest
   ```
4. Access `GET /api/tasks` via browser or HTTP client (OpenAPI available at `/openapi/v1.json`).
5. Run tests:
   ```bash
   dotnet test NSubstitute.UnitTest
   ```
