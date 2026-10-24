# ShopSphere Architecture

## 1. Overview

ShopSphere is a full-stack e-commerce application built with React, ASP.NET Core Web API, and Microsoft SQL Server.

The initial architecture is a modular monolith. The application will run as a cohesive system with clearly separated responsibilities rather than starting with microservices.

## 2. Technology Stack

- Frontend: React, Vite, JavaScript, React Router, Redux Toolkit, RTK Query
- Backend: C#, ASP.NET Core Web API, .NET 10
- Database: Microsoft SQL Server
- ORM: Entity Framework Core
- Testing: xUnit, Moq, integration tests
- Version control: Git and GitHub
- DevOps: Docker and GitHub Actions, introduced in later phases

## 3. Planned Backend Layers

### Domain

Contains core business entities, value objects, and business rules. It should not depend on the database or web framework.

### Application

Coordinates use cases, validation, commands, queries, and application workflows.

### Infrastructure

Implements database access, external service integrations, and persistence using Entity Framework Core and SQL Server.

### API

Exposes HTTP endpoints, handles request and response contracts, and configures authentication, authorization, middleware, and routing.

## 4. Request Flow

React frontend
    -> ASP.NET Core API
    -> Application use case
    -> Domain business rules
    -> Infrastructure implementation
    -> SQL Server

The API will validate business-critical information on the server. The frontend is not authoritative for prices, stock, or payment status.

## 5. Initial Modules

- Identity and access
- Product catalog
- Shopping cart and pricing
- Orders and payments
- Inventory
- Administration
- Supplier product-feed integration

## 6. Initial Development Status

- ASP.NET Core Web API created and launched locally
- React + Vite frontend created and rendered locally
- Git repository created and pushed to GitHub
- README added on GitHub
- SQL Server service detected as running
- Database connectivity and schema not yet verified

## 7. Architecture Decisions

- Start with a modular monolith.
- Use Microsoft SQL Server as the primary database.
- Use Entity Framework Core for data access and schema migrations.
- Use CQRS and MediatR only where they provide a clear benefit.
- Keep controllers thin and business rules outside the API layer.
- Use a payment provider's sandbox for payment integration.
- Introduce infrastructure tools only when a concrete requirement justifies them.

## 8. Security Principles

- Hash passwords using appropriate password-hashing mechanisms.
- Enforce authorization on the server.
- Never trust client-submitted order totals or stock availability.
- Never store raw payment card details.
- Keep secrets out of source control.
