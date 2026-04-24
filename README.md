# apitemplate

A production-ready **.NET 10 Web API** template built on **CQRS with Vertical Slices** and **Controllers**.
Use it as a starting point for new projects, or install it as a `dotnet new` template.

---

## Table of Contents

1. [Overview](#1-overview)
2. [Architecture](#2-architecture)
3. [Project Structure](#3-project-structure)
4. [Getting Started](#4-getting-started)
5. [How to Use as a `dotnet new` Template](#5-how-to-use-as-a-dotnet-new-template)
6. [Adding a New Feature](#6-adding-a-new-feature)
7. [Result Pattern](#7-result-pattern)
8. [Database Abstraction](#8-database-abstraction)
9. [Global Exception Handling](#9-global-exception-handling)
10. [MediatR Behaviors](#10-mediatr-behaviors)
11. [Configuration](#11-configuration)
12. [HTTP Status Codes](#12-http-status-codes)

---

## 1. Overview

| Area | Detail |
|---|---|
| **Framework** | .NET 10 Web API · C# 13 |
| **Architecture** | CQRS + Vertical Slices + Controllers |
| **CQRS Library** | MediatR 12.5 |
| **Result Pattern** | `Result<T>` / `Error` — no exception-based flow control |
| **Global Exceptions** | `IExceptionHandler` → ProblemDetails (RFC 7807) |
| **DB Abstraction** | `IDbConnectionFactory`, `IRepository<T>`, `IUnitOfWork` |
| **BaseApiController** | All standard HTTP helpers with proper status codes |
| **Sample Feature** | `WeatherForecast` (GET + POST) as a vertical slice example |
| **API Docs** | Swagger UI (Swashbuckle) |
| **dotnet new** | `.template.config/template.json` for `dotnet new apitpl` |

---

## 2. Architecture

### Vertical Slice Architecture

Each **feature** is a self-contained vertical slice of the application.
All code related to a feature (command/query, handler, request/response DTOs) lives in the same folder.

```
HTTP Request
     │
     ▼
┌─────────────────────────────────────────────────────────┐
│                    Controller Layer                     │
│  WeatherForecastController : BaseApiController          │
│  ┌──────────────────────────────────────────────────┐   │
│  │  ISender.Send(query/command) ──► MediatR         │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────┐
│               MediatR Pipeline Behaviors                │
│  1. LoggingBehavior     (logs every request/response)   │
│  2. ValidationBehavior  (placeholder for FluentVal.)    │
└─────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────┐
│                   Feature Handler                       │
│  GetWeatherForecastQueryHandler                         │
│  CreateWeatherForecastCommandHandler                    │
│  Returns Result<T> (never throws for expected errors)   │
└─────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────┐
│            Data Layer (abstracted)                      │
│  IRepository<T>  ──►  concrete DB driver               │
│  IUnitOfWork     ──►  (IBM Db2, SQL Server, …)         │
│  IDbConnectionFactory                                   │
└─────────────────────────────────────────────────────────┘
```

### CQRS

- **Command** — mutates state, returns `Result<T>` or `Result`.
- **Query** — reads state, returns `Result<T>`.
- Handlers are discovered automatically by MediatR via assembly scanning.

---

## 3. Project Structure

```
src/
└── Apitemplate/
    ├── Apitemplate.csproj
    ├── Program.cs                          ← Minimal setup via extension methods
    ├── appsettings.json
    ├── appsettings.Development.json
    │
    ├── Common/
    │   ├── Abstractions/
    │   │   ├── IDbConnectionFactory.cs     ← Swap DB driver here
    │   │   ├── IRepository.cs              ← Generic CRUD contract
    │   │   └── IUnitOfWork.cs              ← Unit-of-work contract
    │   │
    │   ├── Behaviors/
    │   │   ├── LoggingBehavior.cs          ← Logs every MediatR request/response
    │   │   └── ValidationBehavior.cs       ← Placeholder for FluentValidation
    │   │
    │   ├── Exceptions/
    │   │   └── GlobalExceptionHandler.cs   ← Catches all unhandled exceptions → 500 ProblemDetails
    │   │
    │   ├── Extensions/
    │   │   ├── ServiceCollectionExtensions.cs  ← DI registrations
    │   │   └── WebApplicationExtensions.cs     ← Middleware pipeline
    │   │
    │   └── Results/
    │       ├── Error.cs                    ← Error record (Code + Message)
    │       └── Result.cs                   ← Result<T> monad
    │
    ├── Controllers/
    │   └── BaseApiController.cs            ← HTTP helpers + Result<T> mapping
    │
    └── Features/
        └── WeatherForecast/
            ├── GetWeatherForecast/
            │   ├── GetWeatherForecastQuery.cs
            │   ├── GetWeatherForecastQueryHandler.cs
            │   └── GetWeatherForecastResponse.cs
            ├── CreateWeatherForecast/
            │   ├── CreateWeatherForecastCommand.cs
            │   ├── CreateWeatherForecastCommandHandler.cs
            │   └── CreateWeatherForecastRequest.cs
            └── WeatherForecastController.cs
```

---

## 4. Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Clone & Run

```bash
git clone https://github.com/porkolabgergo/apitemplate.git
cd apitemplate/src/Apitemplate
dotnet run
```

The API starts on `http://localhost:5005` (Development).
Open your browser at `http://localhost:5005` to see the **Swagger UI**.

### Available Endpoints (sample)

| Method | URL | Description |
|---|---|---|
| `GET` | `/api/weatherforecast` | Get a 5-day forecast (stub) |
| `GET` | `/api/weatherforecast?daysAhead=3` | Get N-day forecast |
| `POST` | `/api/weatherforecast` | Create a forecast entry (stub) |

---

## 5. How to Use as a `dotnet new` Template

### Install the template

```bash
# From the repository root
dotnet new install ./src/Apitemplate

# Or install from a NuGet package (once published)
dotnet new install Apitemplate.Template
```

### Create a new project

```bash
dotnet new apitpl -n MyAwesomeApi
cd MyAwesomeApi
dotnet run
```

The `--name` (`-n`) parameter replaces the `Apitemplate` namespace throughout all files.

### Uninstall

```bash
dotnet new uninstall ./src/Apitemplate
```

---

## 6. Adding a New Feature

Follow this step-by-step guide to add a `Product` feature.

### Step 1 — Create the folder structure

```
Features/
└── Product/
    ├── GetProduct/
    │   ├── GetProductQuery.cs
    │   ├── GetProductQueryHandler.cs
    │   └── GetProductResponse.cs
    ├── CreateProduct/
    │   ├── CreateProductCommand.cs
    │   ├── CreateProductCommandHandler.cs
    │   └── CreateProductRequest.cs
    └── ProductController.cs
```

### Step 2 — Define the Query

```csharp
// Features/Product/GetProduct/GetProductQuery.cs
using Apitemplate.Common.Results;
using MediatR;

namespace Apitemplate.Features.Product.GetProduct;

public sealed record GetProductQuery(Guid Id) : IRequest<Result<GetProductResponse>>;
```

### Step 3 — Define the Response DTO

```csharp
// Features/Product/GetProduct/GetProductResponse.cs
namespace Apitemplate.Features.Product.GetProduct;

public sealed record GetProductResponse(Guid Id, string Name, decimal Price);
```

### Step 4 — Implement the Handler

```csharp
// Features/Product/GetProduct/GetProductQueryHandler.cs
using Apitemplate.Common.Results;
using MediatR;

namespace Apitemplate.Features.Product.GetProduct;

internal sealed class GetProductQueryHandler
    : IRequestHandler<GetProductQuery, Result<GetProductResponse>>
{
    public Task<Result<GetProductResponse>> Handle(
        GetProductQuery request,
        CancellationToken cancellationToken)
    {
        // Replace with a real repository call
        // var product = await _repository.GetByIdAsync(request.Id, cancellationToken);
        // if (product is null) return Result.Failure<GetProductResponse>(
        //     Error.NotFound("Product.NotFound", $"Product {request.Id} was not found."));

        var response = new GetProductResponse(request.Id, "Sample Product", 9.99m);
        return Task.FromResult(Result.Success(response));
    }
}
```

### Step 5 — Create the Controller

```csharp
// Features/Product/ProductController.cs
using Apitemplate.Controllers;
using Apitemplate.Features.Product.GetProduct;
using Microsoft.AspNetCore.Mvc;

namespace Apitemplate.Features.Product;

public sealed class ProductController : BaseApiController
{
    [HttpGet("{id:guid}", Name = "GetProduct")]
    [ProducesResponseType(typeof(GetProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await Sender.Send(new GetProductQuery(id), cancellationToken);
        return HandleResult(result);
    }
}
```

That's it — MediatR picks up the handler automatically via assembly scanning.

---

## 7. Result Pattern

The `Result<T>` type avoids using exceptions for expected failure paths.

### Success

```csharp
// In a handler:
return Task.FromResult(Result.Success(myData));

// In a controller (via BaseApiController):
return HandleResult(result); // → 200 OK with data
```

### Failure

```csharp
// Define a typed error
var error = Error.NotFound("Product.NotFound", "Product 123 was not found.");

return Task.FromResult(Result.Failure<GetProductResponse>(error));

// BaseApiController maps error codes automatically:
// *.NotFound   → 404
// *.Conflict   → 409
// *.Validation → 422
// (anything else) → 400
```

### The Error Record

```csharp
public sealed record Error(string Code, string Message)
{
    public static Error NotFound(string code, string message) => new(code, message);
    public static Error Validation(string code, string message) => new(code, message);
    public static Error Conflict(string code, string message) => new(code, message);
}
```

---

## 8. Database Abstraction

The template ships with three interfaces in `Common/Abstractions/`:

| Interface | Purpose |
|---|---|
| `IDbConnectionFactory` | Creates raw `IDbConnection` instances |
| `IRepository<TEntity>` | Generic CRUD operations |
| `IUnitOfWork` | Atomic commit of multiple repository changes |

### Plugging in IBM Db2

1. Add the Db2 NuGet package to your infrastructure project:
   ```bash
   dotnet add package IBM.Data.Db2
   ```

2. Implement `IDbConnectionFactory`:
   ```csharp
   internal sealed class Db2ConnectionFactory(string connectionString) : IDbConnectionFactory
   {
       public IDbConnection CreateConnection()
       {
           var conn = new IBM.Data.Db2.DB2Connection(connectionString);
           conn.Open();
           return conn;
       }

       public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
       {
           var conn = new IBM.Data.Db2.DB2Connection(connectionString);
           await conn.OpenAsync(cancellationToken);
           return conn;
       }
   }
   ```

3. Register in DI:
   ```csharp
   builder.Services.AddSingleton<IDbConnectionFactory>(
       new Db2ConnectionFactory(builder.Configuration.GetConnectionString("Db2")!));
   ```

4. Implement `IRepository<TEntity>` using Dapper or raw ADO.NET with the factory.

### Plugging in Any Other Database

The same pattern works for SQL Server (`SqlConnection`), PostgreSQL (`NpgsqlConnection`), SQLite, etc.
Swap the concrete `IDbConnectionFactory` implementation and leave all other code untouched.

---

## 9. Global Exception Handling

`GlobalExceptionHandler` implements `IExceptionHandler` and catches **all unhandled exceptions**.

- Returns HTTP **500** with RFC 7807 `ProblemDetails`
- Logs the full exception with a **trace ID**
- The trace ID is included in the response body for client-side correlation

```json
{
  "status": 500,
  "title": "An unexpected error occurred.",
  "detail": "An internal server error has occurred. Please try again later.",
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
  "traceId": "0HN7X4ABCDEF:00000001"
}
```

Registered via `services.AddExceptionHandler<GlobalExceptionHandler>()` and
`app.UseExceptionHandler()` in the pipeline.

---

## 10. MediatR Behaviors

### LoggingBehavior

Automatically wraps every command and query handler:

- Logs **request name** and serialised request on entry
- Logs **elapsed time** (ms) and response on exit
- Logs **exception** if the handler throws

```
info: Handling GetWeatherForecastQuery {"DaysAhead":5}
info: Handled GetWeatherForecastQuery in 3ms <result>
```

### ValidationBehavior (placeholder)

An empty pipeline step ready for **FluentValidation** integration.

To activate:

```bash
dotnet add package FluentValidation.DependencyInjectionExtensions
```

Then register validators and uncomment the validation logic in `ValidationBehavior.cs`.

---

## 11. Configuration

### `appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Apitemplate": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

Add a `ConnectionStrings` section when you plug in a real database:

```json
{
  "ConnectionStrings": {
    "Db2": "Server=myserver:50000;Database=mydb;UID=user;PWD=pass;"
  }
}
```

### Serilog (optional)

The template uses the built-in `ILogger` abstraction, so adding Serilog is non-breaking:

```bash
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
```

```csharp
// Program.cs
builder.Host.UseSerilog((ctx, cfg) =>
    cfg.ReadFrom.Configuration(ctx.Configuration));
```

---

## 12. HTTP Status Codes

All `BaseApiController` helpers and how they map from `Result<T>`:

| Method | HTTP Code | When Used |
|---|---|---|
| `OkResult<T>(data)` | 200 OK | Successful GET |
| `CreatedResult<T>(route, values, data)` | 201 Created | Successful POST |
| `AcceptedResult()` | 202 Accepted | Long-running async operation |
| `NoContentResult()` | 204 No Content | Successful DELETE / no body |
| `BadRequestResult(detail)` | 400 Bad Request | Invalid input (default failure) |
| `NotFoundResult(detail)` | 404 Not Found | Error code ends with `.NotFound` |
| `ConflictResult(detail)` | 409 Conflict | Error code ends with `.Conflict` |
| `UnprocessableResult(detail)` | 422 Unprocessable | Error code ends with `.Validation` |
| `GlobalExceptionHandler` | 500 Internal Server Error | Unhandled exception |

### `HandleResult<T>` mapping

`BaseApiController.HandleResult(result)` inspects the `Error.Code` suffix:

| Error.Code suffix | HTTP Response |
|---|---|
| `*.NotFound` | 404 ProblemDetails |
| `*.Conflict` | 409 ProblemDetails |
| `*.Validation` | 422 ProblemDetails |
| *(anything else)* | 400 ProblemDetails |
| *(success)* | 200 OK with value |
