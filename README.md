# API.OrderManagement Overview
A practical demonstration of modern backend architecture patterns including CQRS, MediatR, and Polly resilience in a .NET Web API

# Project Structure 
```
API.CustomerOrderManagement/
├── API.DTO/                          # Data Transfer Objects (Request/Response models)
├── API.Domain/                       # Domain entities, interfaces, business logic
└── API.Web/                          # Main web application
    ├── Common/                       # Shared utilities (ApiResponse, base classes)
    ├── Controllers/                  # HTTP endpoints (CustomerMaintainer, OrderMaintainer)
    ├── Infrastructure/               # DB context, repositories, DI registrations
    ├── Migrations/                   # EF Core database migrations
    ├── QueryObjects/                 # MediatR Queries and Commands (CQRS handlers)
    ├── Program.cs                    # App bootstrap, DI, middleware
    └── customerorders.db             # SQLite database file
```

# MediateR flow
```
Controller.Send(Query)
         │
         ▼
    MediatR Pipeline
         │
         ├── Behavior              (open to add any behaviour)
         └── Handler               (actual business logic)
                  │
                  ▼
           Returns response
                  │
                  ▼
           Controller returns IActionResult
```

# Polly Resilience — Circuit Breaker
```
Normal traffic
               │
               ▼
         ┌──────────┐
         │  OPEN    │ ◄─── requests pass through normally
         └──────────┘
               │
               │ 3 failures in a row
               ▼
         ┌──────────┐
         │   CLOSE  │ ◄─── all requests fail immediately (no DB hit)
         └──────────┘
               │
               │ after 30 seconds
               ▼
         ┌───────────┐
         │ HALF-OPEN │ ◄─── one test request allowed through
         └───────────┘
               │
        ┌──────┴──────┐
        │             │
     success       failure
        │             │
        ▼             ▼
     CLOSED         OPEN
```

# Full request flow
```
Angular HTTP Request
        │
        ▼
  ASP.NET Controller
  (CustomerMaintainer / OrderMaintainer)
        │
        ▼
  CircuitBreaker.Breaker.ExecuteAsync()
  [Polly checks if circuit is OPEN — if so, fails fast]
        │
        ▼
  Mediator.Send(Query or Command)
        │
        ▼
  MediatR Pipeline
  ├── LoggingBehavior   → logs request name
  ├── ValidationBehavior → validates inputs
  └── Handler (in QueryObjects/)
            │
            ▼
       AppDbContext (Infrastructure/)
            │
            ▼
       SQLite Database (customerorders.db)
            │
            ▼
       Returns DTO (API.DTO/)
            │
            ▼
  Controller wraps in JsonResult / NoContent
            │
            ▼
  Angular receives ApiResponse<T>
```
