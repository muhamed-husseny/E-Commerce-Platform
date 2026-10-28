# E-Commerce Platform

A feature-rich, modern e-commerce platform built with **ASP.NET Core Web API**. This project provides a complete online shopping experience, from browsing products and managing carts to completing orders and user account management.

---

## Features

- **User Authentication & Authorization:** Secure login and registration powered by ASP.NET Core Identity with JWT (JSON Web Tokens).
- **Password Management:** Complete password lifecycle including forgot password, reset password via email token, and change password for authenticated users.
- **Product Catalog:** Browse products by brand and category, with search, sorting, and filtering capabilities.
- **Shopping Cart:** Fast, in-memory shopping cart powered by Redis for real-time basket management and session persistence.
- **Multi-Step Checkout:** Guided checkout flow with shipping address entry, delivery method selection, payment processing, and order review.
- **Order History & Tracking:** Users can view detailed order history and status progression.
- **Delivery Methods:** Configurable shipping options with dynamic delivery timelines and costs.
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

---

## Application Preview

**Home Page:**
<img width="1865" height="800" alt="Home Page" src="https://github.com/user-attachments/assets/535536a8-075b-42c5-97d0-55a9ebc6a7e6" />

**Shopping Page:**
<img width="1818" height="893" alt="Shopping Page" src="https://github.com/user-attachments/assets/a2d11b48-757e-47af-a5a1-79939b7f9729" />

**Cart Page:**
<img width="1825" height="875" alt="Cart Page" src="https://github.com/user-attachments/assets/c3fa9144-8ad9-4e2e-b37e-093b9b9bdb67" />
