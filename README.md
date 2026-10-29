# ShopSphere — E-commerce Platform

ShopSphere is a full-stack e-commerce application built as a practical project for learning and demonstrating modern .NET and React development.

## Technology Stack

- **Backend:** ASP.NET Core Web API, .NET 10, C#
- **Data access:** Entity Framework Core
- **Database:** Microsoft SQL Server
- **Frontend:** React, Vite, JavaScript
- **Testing:** xUnit, EF Core InMemory provider
- **Development tools:** Visual Studio, SQL Server Management Studio, Postman

## Current Features

### Product Catalog

- Products and categories stored in SQL Server.
- Entity Framework Core migrations and sample catalog data.
- Versioned product API route: `/api/v1/products`.
- Product list and product detail endpoints.
- Product DTOs to avoid exposing database entities directly.
- Search by product name, description, or SKU.
- Category filtering.
- Sorting by product ID, name, and price.
- Pagination with page metadata.
- Active-product filtering.
- React catalog connected to the backend API.
- Loading, error, empty, and retry states in the frontend.

## Prerequisites

Install the following before running the project:

- .NET 10 SDK
- Node.js and npm
- Microsoft SQL Server
- SQL Server Management Studio (SSMS), recommended
- Visual Studio with ASP.NET Core development support

## Database Setup

1. Ensure your local SQL Server instance is running.
2. Configure the `ShopSphere` connection string in the appropriate local development settings file.
3. Apply the existing Entity Framework Core migrations using Visual Studio's Package Manager Console or your preferred EF Core tooling.
4. Verify that the `ShopSphereDb` database contains the seeded categories and products.

Do not commit passwords, production credentials, or other secrets.

## Running the Backend

1. Open the solution in Visual Studio.
2. Set `ShopSphere.Api` as the startup project.
3. Confirm the development database connection string is correct.
4. Run the API.
5. Use Postman to test the catalog endpoints.

Example requests:

- `GET /api/v1/products`
- `GET /api/v1/products/1`
- `GET /api/v1/products?search=headphones`
- `GET /api/v1/products?categoryId=1`
- `GET /api/v1/products?sortBy=price&sortOrder=desc`
- `GET /api/v1/products?page=1&pageSize=2`

Use the actual API base URL and port shown by your running application.

## Running the Frontend

1. Open a terminal in the frontend directory containing `package.json`.
2. Install dependencies:

   ```bash
   npm install
   ```

3. Configure `VITE_API_BASE_URL` to point to the running backend API, using the URL and port configured for your local environment.
4. Start the frontend:

   ```bash
   npm run dev
   ```

5. Open the local URL printed by Vite and verify that the product catalog loads.

If the API and frontend use different origins, ensure the backend's development CORS configuration permits the frontend origin.

## Running Automated Tests

1. Open the solution in Visual Studio.
2. Open **Test → Test Explorer**.
3. Select **Run All Tests**.
4. Confirm all tests pass.

The current unit tests cover active-product filtering, search, pagination, invalid pagination and sorting, and product detail retrieval. They use EF Core InMemory, so they do not replace integration tests against SQL Server.

## Current Project Status

- Phase 0: Local application foundation established.
- Phase 1: First working product catalog slice implemented and controller tests passing.
- Next: Phase 2 — improve backend maintainability, validation, error handling, logging, and API documentation.

## Development Notes

This is a learning project. Features and architecture will evolve incrementally. Changes should preserve existing functionality and be verified with automated tests.
