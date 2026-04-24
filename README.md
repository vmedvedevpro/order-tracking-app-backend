# Order Tracking Application — Test Assignment

## Goal

Build a web application for tracking order statuses in real time.

## Functional Requirements

### Backend (.NET 8, ASP.NET Core)

- **Orders API** — create and retrieve orders. Each order has the following fields:
  - `OrderNumber` — unique order identifier
  - `Description` — order description
  - `Status` — one of: `Created`, `Shipped`, `Delivered`, `Cancelled`
  - `CreatedAt` — creation timestamp
  - `UpdatedAt` — last update timestamp
- **Event publishing** — asynchronously publish an event to a message queue (Kafka or RabbitMQ) whenever an order status changes.
- **Real-time notifications** — a subscription service that listens for status-change events and pushes notifications to clients over WebSocket or Server-Sent Events.

### Frontend (React + TypeScript)

- **Order list** — display all orders; include a form to create a new order.
- **Order detail page** — show order details with live status updates via WebSocket or SSE.
- **Tracked orders list** *(optional)* — a watchlist managed with a state manager.

## Tech Stack

### Backend
| Concern | Choice |
|---|---|
| Runtime | .NET 8, ASP.NET Core Web API |
| ORM | EF Core (preferred) |
| Database | PostgreSQL or Microsoft SQL Server |
| Messaging | Kafka or RabbitMQ |
| Logging | Console or file output |

### Frontend
| Concern | Choice |
|---|---|
| Language | TypeScript |
| Framework | React |
| Real-time | WebSocket or Server-Sent Events |
| State management | Redux, Zustand, or equivalent |

## Code Requirements

- Clean, readable code following SOLID principles.
- Clear separation of concerns (layers, components).
- README with instructions on how to run both backend and frontend.

## Optional Extras

- `Dockerfile` and `docker-compose.yml` for one-command startup.
- Unit test coverage.
- XML documentation on public classes, methods, and properties.
- Distributed tracing with OpenTelemetry.