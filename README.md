# Order Tracking App — Backend

ASP.NET Core 8 backend for the Order Tracking application. Exposes a REST API for managing orders and streams real-time status updates to
clients over Server-Sent Events. Built with Clean Architecture, CQRS (MediatR), EF Core + PostgreSQL, and RabbitMQ for integration events.

## Related Repositories

- 🎨 **Frontend** — [order-tracking-app-frontend](https://github.com/vmedvedevpro/order-tracking-app-frontend) — React 19 + TypeScript SPA
  that consumes this API and the SSE stream.
- 🏗️ **Infrastructure** — [order-tracking-app-infrastructure](https://github.com/vmedvedevpro/order-tracking-app-infrastructure) — Docker
  Compose setup that brings up the backend, frontend and dependencies together; use it if you want to run the whole stack with one command
  instead of starting each service by hand. Backend and frontend are wired in as git submodules.

> To run the entire stack locally it is recommended to clone `order-tracking-app-infrastructure` with `--recurse-submodules`.

## Features

- **Orders CRUD** via Minimal API (`POST/PUT/GET /orders`, `GET /orders/{id}`, `DELETE /orders/{id}`) with paginated listing.
- **Real-time order status updates** through Server-Sent Events (`GET /orders/sse`) — a single subscription feeds every connected client.
- **Event-driven model** — domain events and integration events are published to RabbitMQ; the SSE broker fans them out to subscribed HTTP
  clients.
- **CQRS + MediatR** — commands (Create/Update/Delete) and queries (Get/GetById) are separated, with pipeline behaviors for cross-cutting
  concerns (validation, etc.).
- **Request validation** powered by FluentValidation, plugged into the MediatR pipeline.
- **Persistence on EF Core + PostgreSQL** with code-first migrations.
- **Global error handling** via `IExceptionHandler` and RFC 7807 `ProblemDetails` responses, kept in sync with the frontend's typed
  problem-details helpers.
- **OpenAPI / Swagger** for API exploration; the same spec drives the frontend's auto-generated TypeScript client.
- **Observability** — OpenTelemetry tracing and metrics.
- **Production-ready container** — Dockerfile for the WebApi plus a `docker-compose.yml` that provisions PostgreSQL and RabbitMQ for local
  development.

## Tech Stack

### Platform & Language

- .NET 8 / C# 12
- ASP.NET Core Minimal API

### Architecture

- Clean Architecture, split across four projects:
    - `OrderTrackingApp.Backend.Domain` — entities, enums and core domain model;
    - `OrderTrackingApp.Backend.Application` — use cases (CQRS), commands/queries, domain & integration events, behaviors;
    - `OrderTrackingApp.Backend.Infrastructure` — EF Core, RabbitMQ, configuration, telemetry, DI;
    - `OrderTrackingApp.Backend.WebApi` — endpoints, SSE broker, exception handlers, host.
- CQRS via **MediatR**

### Storage & Messaging

- **PostgreSQL 17** + **EF Core**
- **RabbitMQ 4** for integration events between modules/services

### Integrations & Infrastructure

- **Server-Sent Events (SSE)** for push updates to the frontend
- **FluentValidation**
- **OpenTelemetry** (tracing / metrics)
- **Swagger / OpenAPI**
- **Docker / Docker Compose**

## Project Layout

```
src/
├── OrderTrackingApp.Backend.Domain          # Entities, enums, base types
├── OrderTrackingApp.Backend.Application     # CQRS, MediatR, events, validation
├── OrderTrackingApp.Backend.Infrastructure  # EF Core, RabbitMQ, telemetry, DI
└── OrderTrackingApp.Backend.WebApi          # Endpoints, SSE, ExceptionHandlers, host
tests/                                       # Tests
docker-compose.yml                           # Postgres + RabbitMQ for local dev
```

## Prerequisites

- .NET SDK **8.0+**
- Docker / Docker Desktop (for running PostgreSQL and RabbitMQ locally)
- `dotnet-ef` global tool (only if you plan to add or apply migrations manually):
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Configuration

The WebApi reads configuration from `src/OrderTrackingApp.Backend.WebApi/appsettings.json`, environment-specific overrides (
`appsettings.Development.json`) and environment variables. Key settings:

| Setting                       | Description                                   | Example                                                                                |
|-------------------------------|-----------------------------------------------|----------------------------------------------------------------------------------------|
| `ConnectionStrings:Database`  | PostgreSQL connection string used by EF Core. | `Host=localhost;Port=5432;Database=order_tracking;Username=postgres;Password=postgres` |
| `RabbitMqOptions:Host`        | RabbitMQ host name.                           | `localhost`                                                                            |
| `RabbitMqOptions:Port`        | RabbitMQ AMQP port.                           | `5672`                                                                                 |
| `RabbitMqOptions:VirtualHost` | RabbitMQ virtual host.                        | `/`                                                                                    |
| `RabbitMqOptions:Username`    | RabbitMQ user.                                | `guest`                                                                                |
| `RabbitMqOptions:Password`    | RabbitMQ password.                            | `guest`                                                                                |

When running in Docker, the same keys are exposed as environment variables using the standard ASP.NET Core convention with `__` as the
separator, e.g. `ConnectionStrings__Database`, `RabbitMqOptions__Host`.

## Local Development (backend only)

> To run the **entire** application (backend + frontend + infrastructure) with a single command,
> use [order-tracking-app-infrastructure](https://github.com/vmedvedevpro/order-tracking-app-infrastructure).

1. Start dependencies:
   ```bash
   docker compose up -d
   ```
   This brings up:
    - PostgreSQL on `localhost:5432` (`postgres` / `postgres`);
    - RabbitMQ on `localhost:5672`, management UI at `http://localhost:15672` (`guest` / `guest`).

2. Restore and build:
   ```bash
   dotnet restore
   dotnet build
   ```

3. Apply EF Core migrations (first run, or after pulling new migrations):
   ```bash
   dotnet ef database update \
     --project src/OrderTrackingApp.Backend.Infrastructure \
     --startup-project src/OrderTrackingApp.Backend.WebApi
   ```

4. Run the WebApi:
   ```bash
   dotnet run --project src/OrderTrackingApp.Backend.WebApi
   ```

5. Swagger UI is available at the URL printed in the startup logs (typically `http://localhost:5000/swagger` or the HTTPS variant).

## Available Commands

| Command                                                                                                                                       | Description                                          |
|-----------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------|
| `dotnet build`                                                                                                                                | Build the entire solution.                           |
| `dotnet run --project src/OrderTrackingApp.Backend.WebApi`                                                                                    | Start the WebApi locally.                            |
| `dotnet test`                                                                                                                                 | Run all tests in the `tests/` folder.                |
| `dotnet ef migrations add <Name> --project src/OrderTrackingApp.Backend.Infrastructure --startup-project src/OrderTrackingApp.Backend.WebApi` | Add a new EF Core migration.                         |
| `dotnet ef database update --project src/OrderTrackingApp.Backend.Infrastructure --startup-project src/OrderTrackingApp.Backend.WebApi`       | Apply migrations to the configured database.         |
| `docker compose up -d`                                                                                                                        | Start PostgreSQL and RabbitMQ for local development. |
| `docker compose down`                                                                                                                         | Stop and remove the local dependencies.              |

## Docker

A multi-stage Dockerfile (`src/OrderTrackingApp.Backend.WebApi/Dockerfile`) builds and publishes the WebApi on top of the official
`mcr.microsoft.com/dotnet/aspnet:8.0` runtime image. The container exposes ports `8080` (HTTP) and `8081` (HTTPS).

Build and run:

```bash
docker build -f src/OrderTrackingApp.Backend.WebApi/Dockerfile -t order-tracking-backend .

docker run --rm -p 5000:8080 \
  -e ConnectionStrings__Database="Host=host.docker.internal;Port=5432;Database=order_tracking;Username=postgres;Password=postgres" \
  -e RabbitMqOptions__Host=host.docker.internal \
  -e RabbitMqOptions__Port=5672 \
  -e RabbitMqOptions__VirtualHost=/ \
  -e RabbitMqOptions__Username=guest \
  -e RabbitMqOptions__Password=guest \
  order-tracking-backend
```

The API is then available at `http://localhost:5000`. In a Compose setup (see the infrastructure repository) `host.docker.internal` is
replaced with the corresponding service names (e.g. `postgres`, `rabbitmq`).

The SSE endpoint (`/orders/sse`) is designed to be proxied without buffering and with a long read timeout — the frontend's bundled Nginx
config already does this; if you put another reverse proxy in front of the API, make sure to apply the same settings.
