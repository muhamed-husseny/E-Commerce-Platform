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
