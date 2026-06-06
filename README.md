# Financial Stock & Portfolio Management API

A production-ready, high-performance .NET 10 Web API designed for financial stock tracking, portfolio management, and structured user commenting. Built using **Entity Framework Core 10** following clean architectural patterns like **Repository Pattern**, **DTO Mapping (Extension Methods)**, and robust safety mechanisms against modern API vulnerabilities.

---

## Key Features & Architectural Highlights

### 1. Robust Architecture & Design Patterns
* **Repository Pattern:** Complete decoupling of data access logic from controllers using generic-ready async interfaces (`IStockRepository`, `ICommentRepository`, `IPortfolioRepository`).
* **Static Extension Mappers:** Optimized object-to-object mapping (`StockMappers`, `CommentMapper`) implemented via C# Extension Methods. This avoids unnecessary reflection overhead (found in heavy external mappers) and eliminates object allocation tracking bugs.
* **Clean Controller Injections:** Controllers utilize explicit dependency injection, keeping HTTP actions lightweight and strictly adhering to the **Single Responsibility Principle (SRP)**.

### 2. High-Performance EF Core Data Flow
* **Advanced QueryObject Optimization:** Features a custom query binding abstraction that supports seamless **Pagination** (`PageNumber`, `PageSize`), multi-column dynamic **Sorting** (`SortBy`, `IsDescending`), and predictive database-level **Filtering** (`Symbol`, `CompanyName`).
* **Strategic Eager Loading (`.Include`):** Prevents the dangerous `N+1` query execution problem and handles cyclic reference issues cleanly at the repository tier.
* **Deferred Execution Abstraction:** Uses `AsQueryable()` to compose flexible database queries dynamically, fetching records only when necessary to reduce RAM overhead.

### 3. Bulletproof Security & Authentication
* **JWT Custom Infrastructure:** Implements stateless **JSON Web Token (JWT)** authentication via standard HMAC-SHA512 digital signatures.
* **ASP.NET Core Identity Integration:** Native integration with Identity services handling highly secure user registration, automated password hashing, and role-based assignment (`Admin`/`User`).
* **Encapsulated Claim Extensions:** Custom `ClaimsPrincipal` extensions (`User.GetUsername()`) built on top of standard WS-Identity schemas to safely extract contextual username metadata during live authenticated requests.

### 4. Developer Experience & API Diagnostics
* **Scalar Interactive Docs:** Embedded modern API documentation engine via `Scalar.AspNetCore` styled with a premium `DeepSpace` theme, enabling precise live-request testing directly from the development ecosystem.

---

## Tech Stack & Dependencies

* **Runtime:** .NET 10 (C# 14)
* **ORM:** Entity Framework Core 10
* **Identity Management:** Microsoft.AspNetCore.Identity.EntityFrameworkCore
* **Security:** Microsoft.AspNetCore.Authentication.JwtBearer, System.IdentityModel.Tokens.Jwt
* **API Exploration:** Scalar.AspNetCore (OpenAPI / MapOpenApi integration)

---

## System Architecture Breakdown

The project strictly organizes files by separation of concerns:

| Directory / File | Description |
| :--- | :--- |
| **`/Models`** | Core Domain Entities (`Stock`, `Comment`, `Portfolio`, `AppUser`) managing relationships and explicit column configurations (e.g., `decimal(18,2)`). |
| **`/Data`** | Database schema definition (`ApplicationDbContext`) managing complex composite primary keys (`AppUserId` + `StockId`) and default identity roles. |
| **`/Controllers`** | HTTP Request handlers managing model state validation (`ModelState.IsValid`) and standard REST responses (`CreatedAtAction`, `Ok`, `BadRequest`). |
| **`/Repository`** | Asynchronous data persistence layers encapsulating all DbContext queries. |
| **`/DTOs`** | Data Transfer Objects split distinctively into Entity representations, Request creations, and Update schemas to prevent over-posting vulnerabilities. |
| **`/Mappers`** | High-performance, compile-time verified static mapper extensions. |
| **`/Helpers`** | Query parameter binders managing system-wide pagination and search structures. |
| **`Program.cs`** | Central configuration pipeline mapping dependency lifecycles (`AddScoped`), authentication rules, and Scalar middleware. |

---

## Security Practices Implemented
* **Massive Over-Posting Prevention:** Strictly separating raw database Models from DTOs ensures users cannot inject unexpected properties into the system.
* **Defensive Model Validation:** Utilizing strong Data Annotations (`[Required]`, `[MinLength]`, `[MaxLength]`, `[Range]`) across all inbound DTOs to enforce severe length restrictions and input boundaries on critical properties like `Symbol`, `Title`, and `PurchasePrice`.

---

## 📈 Database Schema (Entity Relationships)

* **AppUser ↔ Stock (Many-to-Many):** Managed cleanly via a explicit junction model (`Portfolio`) mapped using Fluent API composite keys.
* **Stock ↔ Comment (One-to-Many):** Configured with clean navigation properties.
* **AppUser ↔ Comment (One-to-Many):** Tracks ownership of comments, loaded cleanly using explicit database joins during API fetch cycles.

---

## 🚦 Getting Started

### Prerequisites
* .NET 10 SDK
* SQL Server / localdb configuration

### Setup
1. Clone the repository:
   ```bash
   git clone [https://github.com/your-username/your-repo-name.git](https://github.com/your-username/your-repo-name.git)
  ```

2. Navigate to the project directory and restore dependencies:

```bash
   cd Financial-API
   dotnet restore

```

3. Configure your JWT Secrets and Connection Strings inside `appsettings.json`:

```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FinancialDb;Trusted_Connection=True;"
     },
     "JWT": {
       "Issuer": "YourIssuer",
       "Audience": "YourAudience",
       "SigningKey": "YourSuperSecretAndExtremelyLongKeyHere1234567890!"
     }
   }

```

4. Run migrations and update the database:

```bash
   dotnet ef database update

```

5. Spin up the application:

```bash
   dotnet run

```
6. Access the live Scalar API interactive reference room via: `https://localhost:your-port/scalar/v1`

```
