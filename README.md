# E-Commerce Platform

A feature-rich, modern e-commerce platform built with **ASP.NET Core 8 Web API** and **Angular**. This project provides a complete online shopping experience, from browsing products and managing carts to completing orders and user account management.

---

## Features

- **User Authentication & Authorization:** Secure login and registration powered by ASP.NET Core Identity with JWT (JSON Web Tokens).
- **Password Management:** Complete password lifecycle including forgot password, reset password via email token, and change password for authenticated users.
- **Product Catalog:** Browse products by brand and category, with search, sorting, and filtering capabilities.
- **Shopping Cart:** Fast, in-memory shopping cart powered by Redis for real-time basket management and session persistence.
- **Multi-Step Checkout:** Guided checkout flow with shipping address entry, delivery method selection, payment processing, and order review.
- **Order History & Tracking:** Users can view detailed order history and status progression.
- **Delivery Methods:** Configurable shipping options with dynamic delivery timelines and costs.
- **Email Service:** SMTP-based email notifications for sending password reset links and order notifications.
- **Product Image Upload:** Upload product images with file format, type, and size validation.
- **User Address Management:** Save and auto-fill profile-linked shipping addresses.
- **Pagination & Caching:** Server-side pagination with client-side caching for optimal performance.
- **Global Error Handling:** Custom exception middleware returning standardized API error responses (`ApiResponse` & `ApiExceptionResponse`) with stack trace suppression in production.

---

## Tech Stack

- **Backend:** ASP.NET Core 8 Web API
- **ORM / Data Access:** Entity Framework Core (EF Core 8)
- **Database:** Microsoft SQL Server
- **Caching & Basket:** Redis (StackExchange.Redis)
- **Authentication:** ASP.NET Core Identity & JWT Bearer Tokens
- **Object Mapping:** AutoMapper
- **Frontend:** Angular

---

## Configuration & Environment Setup

### 1. Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB, Express, or Developer edition)
- [Redis](https://redis.io/) (Local service or Docker container: `docker run -d -p 6379:6379 redis`)

### 2. Configuration Template
A template file [`appsettings.example.json`](appsettings.example.json) is provided in the repository. Use it to configure your local settings.

To start, you can copy the example file to `src/Talabat.APIs/appsettings.Development.json` (which is excluded from Git):
```bash
cp appsettings.example.json src/Talabat.APIs/appsettings.Development.json
```

### 3. Recommended: Manage Secrets with `.NET User Secrets`
For security best practices, sensitive values (such as JWT keys, database credentials, and SMTP passwords) should never be committed to source control. Use the .NET User Secrets manager for local development:

```bash
# Navigate to the API project directory
cd src/Talabat.APIs

# Initialize User Secrets for the project
dotnet user-secrets init

# Set your JWT signing key (minimum 32 characters / 256 bits)
dotnet user-secrets set "JWT:Key" "YOUR_STRONG_SECRET_KEY_MINIMUM_32_CHARACTERS"

# Configure Database Connection Strings
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=TalabatDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
dotnet user-secrets set "ConnectionStrings:IdentityConnection" "Server=localhost;Database=TalabatDbIdentity;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"

# Configure Redis Connection
dotnet user-secrets set "ConnectionStrings:Redis" "localhost:6379"

# (Optional) Configure SMTP Credentials
dotnet user-secrets set "EmailSettings:Host" "smtp.yourprovider.com"
dotnet user-secrets set "EmailSettings:Port" "587"
dotnet user-secrets set "EmailSettings:Username" "your-email@domain.com"
dotnet user-secrets set "EmailSettings:Password" "your-smtp-password"
```

### 4. Database Migration & Data Seeding
The application automatically applies EF Core migrations and seeds initial product brands, categories, products, delivery methods, and default admin accounts on startup.

To manually apply migrations via CLI:
```bash
dotnet ef database update --project src/Talabat.Repository --startup-project src/Talabat.APIs
```

### 5. Running the Application
```bash
# Build the entire solution
dotnet build

# Run the API project
dotnet run --project src/Talabat.APIs
```
Once started, explore the interactive Swagger documentation at `https://localhost:7099/swagger`.

---

## Application Preview

**Home Page:**
<img width="1865" height="800" alt="Home Page" src="https://github.com/user-attachments/assets/535536a8-075b-42c5-97d0-55a9ebc6a7e6" />

**Shopping Page:**
<img width="1818" height="893" alt="Shopping Page" src="https://github.com/user-attachments/assets/a2d11b48-757e-47af-a5a1-79939b7f9729" />

**Cart Page:**
<img width="1825" height="875" alt="Cart Page" src="https://github.com/user-attachments/assets/c3fa9144-8ad9-4e2e-b37e-093b9b9bdb67" />
